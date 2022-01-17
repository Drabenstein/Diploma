using Core.Models.Topics;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Commands;

public class BulkRejectTopics
{
    public record Command(long[] TopicsIds) : IRequest<Unit>;

    public class Handler : IRequestHandler<Command, Unit>
    {
        private readonly DbContext _dbContext;

        public Handler(DbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            var topics = await _dbContext.Set<Topic>()
                .Where(x => request.TopicsIds.Contains(x.Id))
                .ToListAsync(cancellationToken: cancellationToken).ConfigureAwait(false);

            foreach (var topic in topics)
            {
                topic.RejectTopic();
            }

            await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            return Unit.Value;
        }
    }

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(x => x.TopicsIds).NotEmpty();
            RuleFor(x => x.TopicsIds).Must(x => x.Length <= 50)
                .WithMessage("Only 50 topics can be processed at once");
            RuleForEach(x => x.TopicsIds).GreaterThan(0);
        }
    }
}