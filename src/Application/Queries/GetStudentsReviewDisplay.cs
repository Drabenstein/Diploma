using Application.Common;
using Application.Queries.Dtos;
using Core.Models.Reviews;
using Core.Models.Theses;
using Core.Models.Users;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Queries;
public static class GetStudentsReviewDisplay
{
    public record Query(string Email, int ReviewId) : IRequest<StudentsReviewDataDto>;

    public class Handler : IRequestHandler<Query, StudentsReviewDataDto>
    {
        private readonly DbContext _dbContext;

        public Handler(DbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        async Task<StudentsReviewDataDto> IRequestHandler<Query, StudentsReviewDataDto>.Handle(Query request, CancellationToken cancellationToken)
        {
            var user = await _dbContext.Set<Student>().FirstOrDefaultAsync(x => x.Email == request.Email).ConfigureAwait(false);

            var review = await _dbContext.Set<Review>().Include(x => x.ReviewModules).Include(x => x.Reviewer).FirstOrDefaultAsync(x => x.Id == request.ReviewId).ConfigureAwait(false);

            var thesis = await _dbContext.Set<Thesis>()
                .Include(x => x.Reviews)
                .Include(x => x.RealizerStudent)
                .Include(x => x.Topic)
                .ThenInclude(x => x.Supervisor)
                .FirstOrDefaultAsync(x => x.Reviews.Any(y => y.Id == request.ReviewId))
                .ConfigureAwait(false);

            if (user is null || review is null || thesis is null) throw new InvalidOperationException("Cannot display review");

            if (thesis.RealizerStudent != user) throw new InvalidOperationException("This review can not be viewed by the current user");

            return new StudentsReviewDataDto
            {
                ReviewId = request.ReviewId,
                Reviewer = $"{review.Reviewer.FirstName} {review.Reviewer.LastName}".CombineAcademicDegreeAndFullName(review.Reviewer.AcademicDegree.ToString()),
                TopicName = thesis.Topic.Name,
                EnglishTopicName = thesis.Topic.EnglishName,
                Modules = review.ReviewModules.Select(x => new ReviewModuleWithValueDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Type = x.Type.ToString(),
                    Value = x.Value
                })
            };
        }
    }

    public class Validator : AbstractValidator<Query>
    {
        public Validator()
        {
            RuleFor(x => x.Email).EmailAddress();
            RuleFor(x => x.ReviewId).GreaterThan(0);
        }
    }
}
