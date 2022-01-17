using Application.Queries.Dtos;
using Core.Models.Theses;
using Core.Models.Users;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            var user = await _dbContext.Set<Student>().FirstOrDefaultAsync(x => x.Email.Address == request.Email).ConfigureAwait(false);

            var thesis = await _dbContext.Set<Thesis>().FirstOrDefaultAsync(x => x.Id == request.ThesisId).ConfigureAwait(false);

            if (user == null || thesis == null) throw new InvalidOperationException("Cannot find thesis with given data");

            return new MyThesisDto
            {
                Id = request.ThesisId,
                Topic = thesis.Topic.Name,
                Status = thesis.Status.ToString(),
                Reviews = thesis.Reviews
                .OrderBy(x => x.Reviewer.LastName)
                .ThenBy(x => x.Reviewer.FirstName)
                .Select(x => new ReviewForMyThesisDto
                {
                    Id = x.Id,
                    Grade = x.Grade == null ? null : x.Grade.ToString(),
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
