using Application.Common;
using Application.Queries.Dtos;
using Core;
using Dapper;
using FluentValidation;
using MediatR;

namespace Application.Queries;

public static class GetReviewersThesesForFieldOfStudyAndYear
{
    public record Query
    (string Email, long FieldOfStudyId, string YearOfDefence, int Page,
        int ItemsPerPage) : IPagedRequest<ReviewersThesisDto>;
    public class Handler : IRequestHandler<Query, PagedResultDto<ReviewersThesisDto>>
    {
        private const string SqlQuery = "select the.thesis_id as \"Id\", t.\"name\" as \"Name\", t.english_name as \"EnglishName\", supervisor .academic_degree as \"SupervisorDegree\", supervisor.first_name || ' ' || supervisor.last_name as \"SupervisorName\", realizer.first_name || ' ' || realizer.last_name || ' ' || realizer.index_number as \"StudentName\", the.status as \"Status\", supervisor.department as \"Department\" from topic as t join thesis as the on t.topic_id = the.topic_id join review as rev on rev.thesis_id = the.thesis_id join \"user\" as reviewer on reviewer.user_id = rev.reviewer_id join \"user\" as realizer on realizer.user_id = the.realizer_student_id join \"user\" as supervisor on t.supervisor_id = supervisor.user_id where the.status in ('ReadyToReview', 'Reviewed') and rev.grade is null and t.year_of_defence = :YearOfDefence and t.field_of_study_id = :FieldOfStudyId and reviewer.user_id = :TutorId ORDER BY realizer.last_name asc, realizer.first_name asc OFFSET :OffsetRows ROWS FETCH NEXT :ItemsPerPage ROWS ONLY";

        private const string SqlCountQuery = "select count(*) from topic as t join thesis as the on t.topic_id = the.topic_id join review as rev on rev.thesis_id = the.thesis_id join \"user\" as reviewer on reviewer.user_id = rev.reviewer_id join \"user\" as realizer on realizer.user_id = the.realizer_student_id join \"user\" as supervisor on t.supervisor_id = supervisor.user_id where the.status in ('ReadyToReview', 'Reviewed') and rev.grade is null and t.year_of_defence = :YearOfDefence and t.field_of_study_id = :FieldOfStudyId and reviewer.user_id = :TutorId";
     
        private const string TutorIdSql = "SELECT user_id FROM \"user\" WHERE email = :Email";

        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        public Handler(ISqlConnectionFactory sqlConnectionFactory)
        {
            _sqlConnectionFactory =
                sqlConnectionFactory ?? throw new ArgumentNullException(nameof(sqlConnectionFactory));
        }

        public async Task<PagedResultDto<ReviewersThesisDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            using var connection = await _sqlConnectionFactory.CreateOpenConnectionAsync().ConfigureAwait(false);

            var tutorId = await connection.GetUserIdByEmailAsync(request.Email).ConfigureAwait(false);

            var results =
                await connection
                    .QueryAsync<ReviewersThesisDtoWithDegrees>(SqlQuery, new
                    {
                        YearOfDefence = request.YearOfDefence,
                        FieldOfStudyId = request.FieldOfStudyId,
                        OffsetRows = (request.Page - 1) * request.ItemsPerPage,
                        ItemsPerPage = request.ItemsPerPage,
                        TutorId = tutorId
                    }).ConfigureAwait(false);

            var dtos = results.Select(x => new ReviewersThesisDto
            {
                ThesisId = x.ThesisId,
                Title = x.Title,
                EnglishTitle = x.EnglishTitle,
                Supervisor = x.Supervisor.CombineAcademicDegreeAndFullName(x.SupervisorDegree),
                Realizer = x.Realizer,
                Status = x.Status,
                Department = x.Department
            }).ToList();

            return await dtos.GetPagedResultAsync(connection, SqlCountQuery, new
            {
                YearOfDefence = request.YearOfDefence,
                FieldOfStudyId = request.FieldOfStudyId,
                TutorId = tutorId
            }, request.Page, request.ItemsPerPage).ConfigureAwait(false);
        }
    }

    private class ReviewersThesisDtoWithDegrees
    {
        public long ThesisId { get; set; }
        public string Title { get; set; }
        public string EnglishTitle { get; set; }
        public string SupervisorDegree { get; set; }
        public string Supervisor { get; set; }
        public string Realizer { get; set; }
        public string Status { get; set; }
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
