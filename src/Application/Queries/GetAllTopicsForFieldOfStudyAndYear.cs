using Application.Common;
using Application.Queries.Dtos;
using Core;
using Dapper;
using FluentValidation;
using MediatR;

namespace Application.Queries;

public static class GetAllTopicsForFieldOfStudyAndYear
{
    public record Query
    (long FieldOfStudyId, string YearOfDefence, int Page,
        int ItemsPerPage) : IPagedRequest<StudentsTopicDto>;

    public class Handler : IRequestHandler<Query, PagedResultDto<StudentsTopicDto>>
    {
        private const string SqlQuery =
            "select t.topic_id as \"Id\", t.\"name\" as \"TopicName\", t.english_name as \"EnglishTopicName\", supervisor.academic_degree as \"TutorDegree\", supervisor.first_name || ' ' || supervisor.last_name as \"TutorName\", reviewer.academic_degree as \"ReviewerDegree\", reviewer.first_name || ' ' || reviewer.last_name as \"ReviewerName\", t.is_free as \"Status\", supervisor.department as \"Department\" from topic as t join \"user\" as supervisor on t.supervisor_id = supervisor.user_id left join thesis as the on t.topic_id = the.topic_id left join review as rev on rev.thesis_id = the.thesis_id left join \"user\" as reviewer on reviewer.user_id = rev.reviewer_id where t.year_of_defence = :YearOfDefence and t.field_of_study_id = :FieldOfStudyId 	ORDER BY supervisor.last_name asc, supervisor.first_name asc OFFSET :OffsetRows ROWS FETCH NEXT :ItemsPerPage ROWS ONLY";

        private const string SqlCountQuery = @"SELECT COUNT(*)" +
             "from topic as t join \"user\" as supervisor on t.supervisor_id = supervisor.user_id left join thesis as the on t.topic_id = the.topic_id left join review as rev on rev.thesis_id = the.thesis_id left join \"user\" as reviewer on reviewer.user_id = rev.reviewer_id where t.year_of_defence = :YearOfDefence and t.field_of_study_id = :FieldOfStudyId";
        //reviewer.user_id != supervisor.user_id
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        public Handler(ISqlConnectionFactory sqlConnectionFactory)
        {
            _sqlConnectionFactory =
                sqlConnectionFactory ?? throw new ArgumentNullException(nameof(sqlConnectionFactory));
        }

        public async Task<PagedResultDto<StudentsTopicDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            using var connection = await _sqlConnectionFactory.CreateOpenConnectionAsync().ConfigureAwait(false);
            var results =
                await connection
                    .QueryAsync<StudentsTopicDtoWithDegrees>(SqlQuery, new
                    {
                        YearOfDefence = request.YearOfDefence,
                        FieldOfStudyId = request.FieldOfStudyId,
                        OffsetRows = (request.Page - 1) * request.ItemsPerPage,
                        ItemsPerPage = request.ItemsPerPage
                    }).ConfigureAwait(false);

            var dtos = results.Select(x => new StudentsTopicDto
            {
                Id = x.Id,
                TopicName = x.TopicName,
                EnglishTopicName = x.EnglishTopicName,
                TutorName = x.TutorName.CombineAcademicDegreeAndFullName(x.TutorDegree),
                ReviewerName = x.ReviewerName.CombineAcademicDegreeAndFullName(x.ReviewerDegree),
                Status = x.Status,
                Department = x.Department
            }).ToList();

            return await dtos.GetPagedResultAsync(connection, SqlCountQuery, new
            {
                YearOfDefence = request.YearOfDefence,
                FieldOfStudyId = request.FieldOfStudyId
            }, request.Page, request.ItemsPerPage).ConfigureAwait(false);
        }
    }

    private class StudentsTopicDtoWithDegrees
    {
        public long Id { get; set; }
        public string TopicName { get; set; }
        public string EnglishTopicName { get; set; }
        public string TutorDegree { get; set; }
        public string TutorName { get; set; }
        public string ReviewerDegree { get; set; }
        public string ReviewerName { get; set; }
        public string Status { get; set; }
        public string Department { get; set; }
    }

    public class Validator : AbstractValidator<Query>
    {
        public Validator()
        {
            RuleFor(x => x.YearOfDefence).NotEmpty();
            RuleFor(x => x.FieldOfStudyId).GreaterThan(0);
            RuleFor(x => x.Page).GreaterThan(0);
            RuleFor(x => x.ItemsPerPage).GreaterThan(0);
        }
    }


}
