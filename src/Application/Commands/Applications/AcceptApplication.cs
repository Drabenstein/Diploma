using Core.Models.Topics;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using CoreTopics = Core.Models.Topics;

namespace Application.Commands.Applications;

public static class AcceptApplication
{
    public record Command(string TutorEmail, long ApplicationId) : IRequest<bool>;

    public class Handler : IRequestHandler<Command, bool>
    {
        private readonly DbContext _dbContext;

        public Handler(DbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<bool> Handle(Command request, CancellationToken cancellationToken)
        {
            var topic = await _dbContext.Set<Topic>()
                .Include(x => x.Applications)
                .Where(x => x.Supervisor.Email.Address == request.TutorEmail)
                .SingleOrDefaultAsync(x => x.Applications.Any(a => a.Id == request.ApplicationId),
                    cancellationToken: cancellationToken)
                .ConfigureAwait(false);

            if (topic is null)
            {
                return false;
            }

            topic.AcceptApplication(request.ApplicationId);
            await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            return true;
        }
    }

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(x => x.TutorEmail).EmailAddress();
            RuleFor(x => x.ApplicationId).GreaterThan(0);
        }
    }
}