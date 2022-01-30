using Application.Common;
using Application.Queries.Dtos;
using Core.Models.Theses;
using Core.Models.Users;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Queries;
public static class GetMyThesis
{
    public record Query(string Email, long ThesisId) : IRequest<MyThesisDto>;

    public class Handler : IRequestHandler<Query, MyThesisDto>
    {
        private readonly DbContext _dbContext;

        public Handler(DbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<MyThesisDto> Handle(Query request, CancellationToken cancellationToken)
        {
            var user = await _dbContext.Set<Student>().FirstOrDefaultAsync(x => x.Email == request.Email).ConfigureAwait(false);

            var thesis = await _dbContext.Set<Thesis>()
                .Include(x => x.Topic)
                    .ThenInclude(x => x.Supervisor)
                .Include(x => x.Topic)
                    .ThenInclude(x => x.FieldOfStudy)
                .Include(x => x.Reviews)
                    .ThenInclude(x => x.Reviewer)
                .FirstOrDefaultAsync(x => x.Id == request.ThesisId)
                .ConfigureAwait(false);

            if (user is null || thesis is null) throw new InvalidOperationException("Cannot find thesis with given data");
            return new MyThesisDto
            {
                Id = request.ThesisId,
                TopicName = thesis.Topic.Name,
                TopicEnglishName = thesis.Topic.EnglishName,
                Status = thesis.Status.ToString(),
                Language = thesis.Language?.ToString(),
                HasConsentToChangeLanguage = thesis.HasConsentToChangeLanguage,
                SupervisorFullName = ($"{thesis.Topic.Supervisor.FirstName} {thesis.Topic.Supervisor.LastName}".CombineAcademicDegreeAndFullName(thesis.Topic.Supervisor.AcademicDegree.ToString())),
                YearOfDefence = thesis.Topic.YearOfDefence,
                FieldOfStudy = thesis.Topic.FieldOfStudy.Name,
                Reviews = thesis.Reviews
                .OrderBy(x => x.Reviewer.LastName)
                .ThenBy(x => x.Reviewer.FirstName)
                .Select(x => new ReviewForMyThesisDto
                {
                    Id = x.Id,
                    Grade = x.Grade?.ToString(),
                    Timestamp = x.PublishTimestamp,
                    Reviewer = $"{x.Reviewer.FirstName} {x.Reviewer.LastName}"
                })
            };
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
