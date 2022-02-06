using Application.Common;
using Application.Queries.Dtos;
using Core;
using Dapper;
using FluentValidation;
using MediatR;

namespace Application.Queries;

public static class GetAllTopicsForTutorForFieldOfStudyAndYear
{
    public record Query
    (string Email, long FieldOfStudyId, string YearOfDefence, int Page,
        int ItemsPerPage) : IPagedRequest<TutorsTopicDto>;
    public class Handler : IRequestHandler<Query, PagedResultDto<TutorsTopicDto>>
    {
        private const string SqlQuery =
            "select t.topic_id as \"Id\", t.\"name\" as \"TopicName\", t.english_name as \"EnglishTopicName\", realizer.first_name || ' ' || realizer.last_name || ' ' || realizer.index_number as \"StudentName\", reviewer.academic_degree as \"ReviewerAcademicDegree\", reviewer.first_name || ' ' || reviewer.last_name as \"ReviewerName\", t.is_free as \"Free\", supervisor.department as \"Department\"" +
                "from topic as t join \"user\" as supervisor on t.supervisor_id = supervisor.user_id left join thesis as the on t.topic_id = the.topic_id left join review as rev on rev.thesis_id = the.thesis_id left join \"user\" as reviewer on reviewer.user_id = rev.reviewer_id left join \"user\" as realizer on realizer.user_id = the.realizer_student_id where t.year_of_defence = :YearOfDefence and t.field_of_study_id = :FieldOfStudyId and t.supervisor_id = :TutorId and coalesce(reviewer.user_id, 0) <> supervisor.user_id    ORDER BY realizer.last_name asc, realizer.first_name asc OFFSET :OffsetRows ROWS FETCH NEXT :ItemsPerPage ROWS ONLY";
        private const string SqlCountQuery = @"SELECT COUNT(*)" +
             "from topic as t join \"user\" as supervisor on t.supervisor_id = supervisor.user_id left join thesis as the on t.topic_id = the.topic_id left join review as rev on rev.thesis_id = the.thesis_id left join \"user\" as reviewer on reviewer.user_id = rev.reviewer_id left join \"user\" as realizer on realizer.user_id = the.realizer_student_id where t.year_of_defence = :YearOfDefence and t.field_of_study_id = :FieldOfStudyId and t.supervisor_id = :TutorId and coalesce(reviewer.user_id, 0) <> supervisor.user_id";
        
        private const string TutorIdSql = "SELECT user_id FROM \"user\" WHERE email = :Email";

        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        public Handler(ISqlConnectionFactory sqlConnectionFactory)
        {
            _sqlConnectionFactory =
                sqlConnectionFactory ?? throw new ArgumentNullException(nameof(sqlConnectionFactory));
        }

        public async Task<PagedResultDto<TutorsTopicDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            using var connection = await _sqlConnectionFactory.CreateOpenConnectionAsync().ConfigureAwait(false);
            var tutorId = await connection.QuerySingleAsync<long>(TutorIdSql, new { Email = request.Email}).ConfigureAwait(false);

            var results =
                await connection
                    .QueryAsync<TutorsWithDegreeTopicDto>(SqlQuery, new
                    {
                        YearOfDefence = request.YearOfDefence,
                        FieldOfStudyId = request.FieldOfStudyId,
                        OffsetRows = (request.Page - 1) * request.ItemsPerPage,
                        ItemsPerPage = request.ItemsPerPage,
                        TutorId = tutorId
                    }).ConfigureAwait(false);

            var dtos = results.Select(x => new TutorsTopicDto
            {
                Id = x.Id,
                TopicName = x.TopicName,
                EnglishTopicName = x.EnglishTopicName,
                StudentName = x.StudentName,
                ReviewerName = x.ReviewerName.CombineAcademicDegreeAndFullName(x.ReviewerAcademicDegree),
                Status = x.Free,
                Department = x.Department
            });

            return await dtos.GetPagedResultAsync(connection, SqlCountQuery, new
            {
                YearOfDefence = request.YearOfDefence,
                FieldOfStudyId = request.FieldOfStudyId,
                TutorId = tutorId
            }, request.Page, request.ItemsPerPage).ConfigureAwait(false);
        }
    }

    private class TutorsWithDegreeTopicDto
    {
        public long Id { get; set; }
        public string TopicName { get; set; }
        public string EnglishTopicName { get; set; }
        public string StudentName { get; set; }
        public string ReviewerAcademicDegree { get; set; }
        public string ReviewerName { get; set; }
        public string Free { get; set; }
        public string Department { get; set; }
    }

    public class Validator : AbstractValidator<Query>
    {
        public Validator()
        {
            RuleFor(x => x.Email).EmailAddress();
            RuleFor(x => x.YearOfDefence).NotEmpty();
            RuleFor(x => x.FieldOfStudyId).GreaterThan(0);
            RuleFor(x => x.Page).GreaterThan(0);
            RuleFor(x => x.ItemsPerPage).GreaterThan(0);
        }
    }


}
