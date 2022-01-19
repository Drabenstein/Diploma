using Application.Queries.Dtos;
using Core.Models.Users;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Queries;
public static class GetMyApprovedTopics
{
    public record Query(string Email) : IRequest<IEnumerable<ApprovedTopicDto>>;

    public class Handler : IRequestHandler<Query, IEnumerable<ApprovedTopicDto>>
    {
        private readonly DbContext _dbContext;

        public Handler(DbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<IEnumerable<ApprovedTopicDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            var user = await _dbContext.Set<Student>().FirstOrDefaultAsync(x => x.Email == request.Email).ConfigureAwait(false);

            if (user == null) throw new InvalidOperationException("Invalid user data");

            return await _dbContext
                .Set<Core.Models.Topics.Application>()
                .Where(x => x.Submitter.Id == user.Id)
                .Where(x => x.Status == Core.Models.Topics.ValueObjects.ApplicationStatus.Approved)
                .Select(x => new ApprovedTopicDto
                {
                    ApplicationId = x.Id,
                    TopicId = x.Topic.Id,
                    Name = x.Topic.Name,
                    EnglishName = x.Topic.EnglishName,
                    Supervisor = $"{x.Topic.Supervisor.AcademicDegree} {x.Topic.Supervisor.FirstName} {x.Topic.Supervisor.LastName}",
                    ApplicationStatus = x.Status.ToString()
                })
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);
        }
    }

    public class Validator : AbstractValidator<Query>
    {
        public Validator()
        {
            RuleFor(x => x.Email).EmailAddress();
        }
    }
}

