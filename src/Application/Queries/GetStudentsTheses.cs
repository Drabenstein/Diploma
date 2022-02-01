using Application.Common;
using Application.Queries.Dtos;
using Core;
using Dapper;
using FluentValidation;
using MediatR;

namespace Application.Queries;

public static class GetStudentsTheses
{
    public record Query(string Email) : IRequest<IEnumerable<StudentsThesisDto>>;

    public class Handler : IRequestHandler<Query, IEnumerable<StudentsThesisDto>>
    {
        private const string Sql = @"
            select the.thesis_id as Id, top." + "\"name\"" + " as TopicName, top." + "\"english_name\"" + @" as TopicEnglishName, sup.first_name || sup.last_name as SupervisorName, sup.academic_degree as SupervisorDegree, the.status as Status
	        from thesis as the join topic as top on the.topic_id = top.topic_id
            join " + "\"user\"" + @" as sup on top.supervisor_id = sup.user_id
            where the.realizer_student_id = :UserId
        ";

        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        public Handler(ISqlConnectionFactory sqlConnectionFactory)
        {
            _sqlConnectionFactory =
                sqlConnectionFactory ?? throw new ArgumentNullException(nameof(sqlConnectionFactory));
        }

        public async Task<IEnumerable<StudentsThesisDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            using var connection = await _sqlConnectionFactory.CreateOpenConnectionAsync().ConfigureAwait(false);

            var userId = await connection.GetUserIdByEmailAsync(request.Email).ConfigureAwait(false);

            var dtos = await connection.QueryAsync<StudentThesisWithSupervisorData>(Sql, new {UserId = userId}).ConfigureAwait(false);

            return dtos.Select(x => new StudentsThesisDto
            {
                Id = x.Id,
                TopicName = x.TopicName,
                TopicEnglishName = x.TopicEnglishName,
                Supervisor = x.SupervisorName.CombineAcademicDegreeAndFullName(x.SupervisorDegree),
                Status = x.Status
            }).ToList();
        }

        private class StudentThesisWithSupervisorData
        {
            public int Id { get; set; }
            public string TopicName { get; set; }
            public string TopicEnglishName { get; set; }
            public string SupervisorName { get; set; }
            public string SupervisorDegree { get; set; }
            public string Status { get; set; }
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

