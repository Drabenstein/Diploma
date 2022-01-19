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

public static class GetUserThesisId
{
    public record Query(string Email) : IRequest<long>;

    public class Handler : IRequestHandler<Query, long>
    {
        private readonly DbContext _dbContext;

        public Handler(DbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<long> Handle(Query request, CancellationToken cancellationToken)
        {
            var user = await _dbContext.Set<Student>().FirstOrDefaultAsync(x => x.Email == request.Email).ConfigureAwait(false);
            
            if (user == null) throw new InvalidOperationException("User not found");

            var thesisId = await _dbContext.Set<Thesis>()
                .Where(x => x.RealizerStudent.Id == user.Id)
                .Select(x => x.Id)
                .FirstOrDefaultAsync(cancellationToken)
                .ConfigureAwait(false);

            return thesisId;
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

