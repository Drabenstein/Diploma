using Application.Common;
using Application.Queries.Dtos;
using Core;
using Dapper;
using FluentValidation;
using MediatR;

namespace Application.Queries;

public static class GetTopicsForConsiderationByYearOfDefenceAndField
{
    public record Query
        (long FieldOfStudyId, string YearOfDefence, int Page, int ItemsPerPage) : IRequest<
            PagedResultDto<TopicForConsiderationDto>>;

    public class Handler : IRequestHandler<Query, PagedResultDto<TopicForConsiderationDto>>
    {
        private const string SqlQuery = @"
                SELECT  t.topic_id AS Id,
                        t.name AS Name,
                        t.english_name AS EnglishName,
                        s.academic_degree AS SupervisorAcademicDegree,
                        s.first_name || ' ' || s.last_name AS SupervisorFullName,
                        t.max_realization_number AS MaxRealizationNumber,
                        s.department AS SupervisorDepartment
                FROM topic t
                    JOIN " + "\"user\"" + @" s ON t.supervisor_id = s.user_id
                WHERE t.field_of_study_id = :FieldOfStudyId
                    AND t.year_of_defence = :YearOfDefence
                    AND t.is_accepted IS NULL
                    AND (t.is_proposed_by_student = 0 OR EXISTS (
                            SELECT * FROM application a
                            WHERE a.topic_id = t.topic_id
                            AND a.status = 'Accepted'
                        ))
                ORDER BY s.last_name, s.first_name, s.department
                OFFSET :OffsetRows ROWS FETCH NEXT :ItemsPerPage ROWS ONLY";

        private readonly string CountQuery = @"
                SELECT COUNT(*)
                FROM topic t
                WHERE t.field_of_study_id = :FieldOfStudyId
                    AND t.year_of_defence = :YearOfDefence
                    AND t.is_accepted IS NULL
                    AND (t.is_proposed_by_student = 0 OR EXISTS (
                            SELECT * FROM application a
                            WHERE a.topic_id = t.topic_id
                                AND a.status = 'Accepted'
                        ))";

        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        public Handler(ISqlConnectionFactory sqlConnectionFactory)
        {
            _sqlConnectionFactory =
                sqlConnectionFactory ?? throw new ArgumentNullException(nameof(sqlConnectionFactory));
        }

        public async Task<PagedResultDto<TopicForConsiderationDto>> Handle(Query request,
            CancellationToken cancellationToken)
        {
            using var connection = await _sqlConnectionFactory.CreateOpenConnectionAsync().ConfigureAwait(false);
            var results = await connection.QueryAsync<TopicForConsiderationDto>(SqlQuery, new
                {
                    FieldOfStudyId = request.FieldOfStudyId,
                    YearOfDefence = request.YearOfDefence,
                    OffsetRows = (request.Page - 1) * request.ItemsPerPage,
                    ItemsPerPage = request.ItemsPerPage
                })
                .ConfigureAwait(false);

            return await results.GetPagedResultAsync(connection, CountQuery, new
                {
                    FieldOfStudyId = request.FieldOfStudyId,
                    YearOfDefence = request.YearOfDefence
                }, request.Page, request.ItemsPerPage)
                .ConfigureAwait(false);
        }
    }

    public class Validator : AbstractValidator<Query>
    {
        public Validator()
        {
            RuleFor(x => x.FieldOfStudyId).GreaterThan(0);
            RuleFor(x => x.YearOfDefence).NotEmpty();
            RuleFor(x => x.ItemsPerPage).GreaterThan(0);
            RuleFor(x => x.Page).GreaterThan(0);
        }
    }
}