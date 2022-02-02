using Application.Commands.Dtos;
using Core.Models.Reviews;
using Core.Models.Theses;
using Core.Models.Users;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Commands;
public static class SubmitReview
{
    public record Command(string UserEmail, FilledReviewModuleDto[] ReviewModules, int ReviewId, string Grade) : IRequest<Unit>;

    public class Handler : IRequestHandler<Command, Unit>
    {
        private readonly DbContext _dbContext;

        public Handler(DbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            var user = await _dbContext.Set<Tutor>()
                .FirstOrDefaultAsync(x => x.Email == request.UserEmail)
                .ConfigureAwait(false);

            var review = await _dbContext.Set<Review>()
                .FirstOrDefaultAsync(x => x.Id == request.ReviewId)
                .ConfigureAwait(false);

            var reviewModules = await _dbContext.Set<Review>()
                .Include(x => x.ReviewModules)
                .Where(x => x.Id == request.ReviewId)
                .Select(x => x.ReviewModules)
                .FirstOrDefaultAsync()
                .ConfigureAwait(false);

            var thesis = await _dbContext.Set<Thesis>()
                .Include(x => x.Reviews)
                .Where(x => x.Reviews.Any(y => y.Id == request.ReviewId))
                .FirstOrDefaultAsync()
                .ConfigureAwait(false);

            if (review?.Reviewer.Id != user?.Id)
            {
                throw new InvalidOperationException("You can only post your own reviews");
            }

            if (reviewModules is null || review is null)
            {
                throw new InvalidOperationException("Cannot post review, no review modules found");
            }

            if (thesis is null)
            {
                throw new InvalidOperationException("No thesis found to add review");
            }

            foreach (var module in request.ReviewModules)
            {
                reviewModules.FirstOrDefault(x => x.Id == module.Id)?.SetValue(module.Value);
            }

            thesis.ReviewThesis(request.Grade, request.ReviewId);

            await _dbContext.SaveChangesAsync().ConfigureAwait(false);

            return Unit.Value;
        }
    }

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(x => x.ReviewId).GreaterThan(0);
            RuleFor(x => x.UserEmail).EmailAddress();
            RuleFor(x => x.Grade).NotEmpty();
            RuleForEach(x => x.ReviewModules).ChildRules(x =>
            {
                x.RuleFor(y => y.Id).GreaterThan(0);
                x.RuleFor(y => y.Name).NotEmpty();
                x.RuleFor(y => y.Value).NotEmpty();
                x.RuleFor(y => y.Type).NotEmpty();
            });
        }
    }
}
