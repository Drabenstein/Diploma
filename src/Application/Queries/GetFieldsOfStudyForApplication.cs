using Application.Queries.Dtos;
using Core.Models.Topics;
using Core.Models.Users;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Queries;

public static class GetFieldsOfStudyForApplication
{
    public record Query(string Email) : IRequest<IEnumerable<FieldOfStudyForApplicationDto>>;

    public class Handler : IRequestHandler<Query, IEnumerable<FieldOfStudyForApplicationDto>>
    {
        private readonly DbContext _dbContext;

        public Handler(DbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<IEnumerable<FieldOfStudyForApplicationDto>> Handle(Query request, CancellationToken cancellationToken)
        {

            var user = await _dbContext.Set<Student>().FirstOrDefaultAsync(x => x.Email.Address == request.Email, cancellationToken).ConfigureAwait(false);

            return await _dbContext.Set<FieldOfStudy>()
                .Where(x => x.StudentFieldsOfStudy.Select(y => y.StudentId).Contains(user.Id))
                .Select(x => new FieldOfStudyForApplicationDto
                    {
                        Id = x.Id,
                        Name = x.Name
                    }).ToListAsync(cancellationToken).ConfigureAwait(false);
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
