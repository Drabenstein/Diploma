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
            "select t.topic_id as \"Id\", t.\"name\" as \"Name\", t.english_name as \"EnglishName\", supervisor.academic_degree || ' ' || supervisor.first_name || ' ' || supervisor.last_name as \"TutorName\", reviewer.academic_degree || ' ' || reviewer.first_name || ' ' || reviewer.last_name as \"ReviewerName\", t.is_free as \"Free\", supervisor.department as \"Department\"" +
                "from topic as t left join \"user\" as supervisor on t.supervisor_id = supervisor.user_id" +
                @"join thesis as the on t.topic_id = the.topic_id
                join review as rev on rev.thesis_id = the.thesis_id" +
                "join \"user\" as reviewer on reviewer.user_id = rev.reviewer_id where reviewer.user_id != supervisor.user_id and t.year_of_defence = :YearOfDefence and t.field_of_study_id = :FieldOfStudyId 	ORDER BY t.is_free desc, t.topic_id DESC OFFSET :OffsetRows ROWS FETCH NEXT :ItemsPerPage ROWS ONLY";

        private const string SqlCountQuery = @"SELECT COUNT(*)" +
             "from topic as t left join \"user\" as supervisor on t.supervisor_id = supervisor.user_id" +
                @"join thesis as the on t.topic_id = the.topic_id
                join review as rev on rev.thesis_id = the.thesis_id" +
                "join \"user\" as reviewer on reviewer.user_id = rev.reviewer_id where reviewer.user_id != supervisor.user_id and t.year_of_defence = :YearOfDefence and t.field_of_study_id = :FieldOfStudyId";
        
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
                    .QueryAsync<StudentsTopicDto>(SqlQuery, new
                    {
                        YearOfDefence = request.YearOfDefence,
                        FieldOfStudyId = request.FieldOfStudyId,
                        OffsetRows = (request.Page - 1) * request.ItemsPerPage,
                        ItemsPerPage = request.ItemsPerPage
                    }).ConfigureAwait(false);

            return await results.GetPagedResultAsync(connection, SqlCountQuery, new
            {
                YearOfDefence = request.YearOfDefence,
                FieldOfStudyId = request.FieldOfStudyId
            }, request.Page, request.ItemsPerPage).ConfigureAwait(false);
        }
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
