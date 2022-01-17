using Application.Queries.Dtos;
using Core.Models.Theses;
using Core.Models.Users;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Queries;
public static class GetDataForDeclaration
{
    public record Query(string Email, long ThesisId) : IRequest<DeclarationDataDto>;

    public class Handler : IRequestHandler<Query, DeclarationDataDto>
    {
        private readonly DbContext _dbContext;

        public Handler(DbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<DeclarationDataDto> Handle(Query request, CancellationToken cancellationToken)
        {
            var user = await _dbContext.Set<Student>().FirstOrDefaultAsync(x => x.Email.Address == request.Email).ConfigureAwait(false);

            if (user == null) throw new InvalidOperationException("Cannot get data, user does not exist");

            return await _dbContext.Set<Thesis>()
                .Where(x => x.Id == request.ThesisId)
                .Where(x => x.RealizerStudent.Id == user.Id)
                .Select(x => new DeclarationDataDto
                {
                    ThesisId = x.Id,
                    Name = x.Topic.Name,
                    EnglishName = x.Topic.EnglishName,
                    FieldOfStudyData = $"{x.Topic.FieldOfStudy.Name}, {x.Topic.FieldOfStudy.StudyForm}, {x.Topic.FieldOfStudy.Degree}",
                    StudentName = $"{x.RealizerStudent.FirstName} {x.RealizerStudent.LastName} {x.RealizerStudent.IndexNumber}",
                    SupervisorName = $"{x.Topic.Supervisor.AcademicDegree} {x.Topic.Supervisor.FirstName} {x.Topic.Supervisor.LastName}, {x.Topic.Supervisor.Department}"
                }).FirstAsync()
                .ConfigureAwait(false);
        }
    }
}
