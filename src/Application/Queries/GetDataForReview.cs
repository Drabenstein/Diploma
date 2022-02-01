using Application.Queries.Dtos;
using Core.Models.Theses;
using Core.Models.Users;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Queries;
public static class GetDataForReview
{
    public record Query(string Email, long ThesisId) : IRequest<ReviewDataDto>;

    public class Handler : IRequestHandler<Query, ReviewDataDto>
    {
        private readonly DbContext _dbContext;

        public Handler(DbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<ReviewDataDto> Handle(Query request, CancellationToken cancellationToken)
        {
            var user = await _dbContext.Set<Tutor>().FirstOrDefaultAsync(x => x.Email == request.Email).ConfigureAwait(false);

            if (user == null) throw new InvalidOperationException("Current user not found");

            return await _dbContext.Set<Thesis>()
                .Where(x => x.Id == request.ThesisId)
                .Select(x => new ReviewDataDto
                {
                    ReviewId = x.Reviews.Where(x => x.Reviewer.Id == user.Id).First().Id,
                    ThesisId = x.Id,
                    Realizer = $"{x.RealizerStudent.FirstName} {x.RealizerStudent.LastName} {x.RealizerStudent.IndexNumber}",
                    Supervisor = $"{x.Topic.Supervisor.AcademicDegree} {x.Topic.Supervisor.FirstName} {x.Topic.Supervisor.LastName}",
                    Topic = $"{x.Topic.Name}",
                    Modules = x.Reviews.Where(x => x.Reviewer.Id == user.Id).First().ReviewModules.Select(y => new ReviewModuleDto
                    {
                        Id = y.Id,
                        Name = y.Name,
                        Type = y.Type
                    }).ToList()
                }).FirstAsync(cancellationToken).ConfigureAwait(false);
        }
    }

    public class Validator : AbstractValidator<Query>
    {
        public Validator()
        {
            RuleFor(x => x.Email).EmailAddress();
            RuleFor(x => x.ThesisId).GreaterThan(0);
        }
    }
}
