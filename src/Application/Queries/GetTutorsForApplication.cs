using Application.Queries.Dtos;
using Core.Models.Users;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Queries;

public static class GetTutorsForApplication
{
    public record Query() : IRequest<IEnumerable<TutorForApplicationDto>>;

    public class Handler : IRequestHandler<Query, IEnumerable<TutorForApplicationDto>>
    {
        private readonly DbContext _dbContext;

        public Handler(DbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<IEnumerable<TutorForApplicationDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            return await _dbContext.Set<Tutor>().Select(x => new TutorForApplicationDto
            {
                Id = x.Id,
                FirstName = x.FirstName,
                LastName = x.LastName,
                AcademicDegree = x.AcademicDegree.ToString()
            }).ToListAsync().ConfigureAwait(false);
        }

    }
}

