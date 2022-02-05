using Core.Models.Topics;
using Core.Models.Users;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Commands;

public static class ApplyForTopic
{
    public record Command(string UserEmail, long TopicId, string Message) : IRequest<Unit>;

    public class Handler : IRequestHandler<Command, Unit>
    {
        private readonly DbContext _dbContext;

        public Handler(DbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            var user = await _dbContext.Set<Student>()
                .FirstOrDefaultAsync(x => x.Email == request.UserEmail, cancellationToken: cancellationToken);

            var topic = _dbContext.Set<Topic>().FirstOrDefault(x => x.Id == request.TopicId);

            if (user is null || topic is null)
                throw new InvalidOperationException("Cannot apply for thesis with given data");

            var application = new Core.Models.Topics.Application
            {
                Submitter = user,
                Topic = topic,
                Timestamp = DateTime.UtcNow,
                Message = request.Message,
                IsTopicProposal = false
            };

            topic.SubmitApplication(application);

            await _dbContext.Set<Core.Models.Topics.Application>().AddAsync(application, cancellationToken)
                .ConfigureAwait(false);
            await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            return Unit.Value;
        }
    }
}