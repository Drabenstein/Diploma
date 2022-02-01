using Core.Models.Users;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Commands;
public static class DeclareThesisReadyForReview
{
    public record Command(string Email, int ThesisId) : IRequest<Unit>;

    public class Handler : IRequestHandler<Command, Unit>
    {
        private readonly DbContext _dbContext;

        public Handler(DbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            var user = await _dbContext.Set<Student>().Include(x => x.Theses).FirstOrDefaultAsync(x => x.Email == request.Email, cancellationToken).ConfigureAwait(false);

            if (user is null) throw new InvalidOperationException("Invalid user data");

            var thesis = user.Theses.FirstOrDefault(x => x.Id == request.ThesisId);

            if (thesis is null) throw new InvalidOperationException("You can only declare your own thesis as ready for review");

            thesis.DeclareAsReadyForReview();

            await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            return Unit.Value;
        }
    }
    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(x => x.Email).EmailAddress();
            RuleFor(x => x.ThesisId).GreaterThan(0);
        }
    }

}

