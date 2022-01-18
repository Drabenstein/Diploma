using System.Collections.Immutable;
using Application.Commands.Dtos;
using Core.Models.Theses;
using Core.Models.Users;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Commands;

public static class BulkChangeReviewers
{
    public record Command(ReviewerChangeDto[] ReviewerChangeDtos) : IRequest<Unit>;

    public class Handler : IRequestHandler<Command, Unit>
    {
        private readonly DbContext _dbContext;

        public Handler(DbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            var requestedChangesDictionary =
                request.ReviewerChangeDtos
                    .ToImmutableDictionary(x => x.ThesisId, x => x.ReviewerId);

            var thesesToChangeIds = requestedChangesDictionary.Keys.ToArray();

            var theses = await _dbContext.Set<Thesis>()
                .Include(x => x.Topic)
                .ThenInclude(x => x.Supervisor)
                .Include(x => x.Reviews)
                .ThenInclude(x => x.Reviewer)
                .Where(x => thesesToChangeIds.Contains(x.Id))
                .ToListAsync(cancellationToken: cancellationToken)
                .ConfigureAwait(false);

            var reviewersIds = requestedChangesDictionary.Values.ToArray();

            var reviewers = await _dbContext.Set<Tutor>()
                .Where(x => reviewersIds.Contains(x.Id))
                .ToListAsync(cancellationToken: cancellationToken).ConfigureAwait(false);

            foreach (var thesis in theses)
            {
                var desiredReviewerId = requestedChangesDictionary[thesis.Id];
                var desiredReviewer = reviewers.First(x => x.Id == desiredReviewerId);
                thesis.ChangeReviewer(desiredReviewer);
            }

            await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            return Unit.Value;
        }
    }

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(x => x.ReviewerChangeDtos).NotEmpty();
            RuleFor(x => x.ReviewerChangeDtos).Must(x => x.Length <= 50)
                .WithMessage("Only 50 reviewers can be changed at once");
            RuleForEach(x => x.ReviewerChangeDtos).ChildRules(x =>
            {
                x.RuleFor(y => y.ThesisId).GreaterThan(0);
                x.RuleFor(y => y.ReviewerId).GreaterThan(0);
            });
        }
    }
}