using Application.Queries.Dtos;
using Core.Models.Topics;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Queries;

public static class GetAllFieldsOfStudy
{
    public record Query() : IRequest<IEnumerable<FieldOfStudyForApplicationDto>>;

    public class Handler : IRequestHandler<Query, IEnumerable<FieldOfStudyForApplicationDto>>
    {
        private readonly DbContext _dbContext;

        public Handler(DbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<IEnumerable<FieldOfStudyForApplicationDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            return await _dbContext.Set<FieldOfStudy>()
                .OrderBy(x => x.Name)
                .Select(x => new FieldOfStudyForApplicationDto
                {
                    Id = x.Id,
                    Name = x.Name
                })
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);
        }
    }
}

