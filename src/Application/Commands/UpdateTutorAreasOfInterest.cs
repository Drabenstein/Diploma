using Core.Models.Users;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Commands;

public static class UpdateTutorAreasOfInterest
{
    public record Command(long[] AreasOfInterestIds, string email) : IRequest<Unit>;

    public class Handler : IRequestHandler<Command, Unit>
    {
        private readonly DbContext _dbContext;

        public Handler(DbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            var user = await _dbContext.Set<User>().Include(x => x.AreasOfInterest)
                .FirstOrDefaultAsync(x => x.Email == request.email, cancellationToken: cancellationToken)
                .ConfigureAwait(false);

            var userAreas = user?.AreasOfInterest.ToList();

            var newAreas = await _dbContext.Set<AreaOfInterest>().Where(x => request.AreasOfInterestIds.Contains(x.Id))
                .ToListAsync().ConfigureAwait(false);

            foreach (var area in newAreas.Except(userAreas ?? new List<AreaOfInterest>()))
            {
                user?.AddAreaOfInterest(area);
            }

            foreach (var area in userAreas.Except(newAreas))
            {
                user?.RemoveAreaOfInterest(area);
            }

            await _dbContext.SaveChangesAsync().ConfigureAwait(false);
            return Unit.Value;
        }
    }

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(x => x.email).EmailAddress();
            RuleForEach(x => x.AreasOfInterestIds).GreaterThan(0);
        }
    }
}