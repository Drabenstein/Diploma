using Application.Common;
using Application.Queries.Dtos;
using Core;
using Dapper;
using FluentValidation;
using MediatR;

namespace Application.Queries;

public static class GetApplicationForYearOfDefenceAndField
{
    public record Query
        (string TutorEmail, long FieldOfStudyId, string YearOfDefence, int Page, int ItemsPerPage) : IRequest<PagedResultDto<ApplicationDto>>;
    
    public class Handler : IRequestHandler<Query, PagedResultDto<ApplicationDto>>
    {
        private const string SqlQuery =
            "SELECT a.application_id AS Id, t.name AS Name, t.english_name AS EnglishName, u.first_name || ' ' || u.last_name AS StudentFullName, a.is_topic_proposal AS IsTopicProposal FROM topic t JOIN application a ON t.topic_id = a.topic_id JOIN \"user\" u ON a.submitter_id = u.user_id WHERE t.supervisor_id = :TutorId AND a.status = 'Sent' AND t.field_of_study_id = :FieldOfStudyId AND t.year_of_defence = :YearOfDefence ORDER BY a.\"timestamp\" DESC OFFSET :OffsetRows ROWS FETCH NEXT :ItemsPerPage ROWS ONLY";
        private const string SqlCountQuery = @"
            SELECT COUNT(*)
            FROM topic t
                JOIN application a
                    ON t.topic_id = a.topic_id
            WHERE t.supervisor_id = :TutorId
                AND a.status = 'Sent'
                AND t.year_of_defence = :YearOfDefence
                AND t.field_of_study_id = :FieldOfStudyId";
        
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        public Handler(ISqlConnectionFactory sqlConnectionFactory)
        {
            _sqlConnectionFactory = sqlConnectionFactory ?? throw new ArgumentNullException(nameof(sqlConnectionFactory));
        }

        public async Task<PagedResultDto<ApplicationDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            using var connection = await _sqlConnectionFactory.CreateOpenConnectionAsync().ConfigureAwait(false);
            var tutorId = await connection.GetUserIdByEmailAsync(request.TutorEmail).ConfigureAwait(false);
            
            var results =
                await connection
                    .QueryAsync<ApplicationDto>(SqlQuery, new
                    {
                        TutorId = tutorId,
                        request.FieldOfStudyId,
                        request.YearOfDefence,
                        OffsetRows = (request.Page - 1) * request.ItemsPerPage,
                        request.ItemsPerPage
                    }).ConfigureAwait(false);

            return await results.GetPagedResultAsync(connection, SqlCountQuery, new {
                TutorId = tutorId,
                request.FieldOfStudyId,
                request.YearOfDefence
            }, request.Page, request.ItemsPerPage).ConfigureAwait(false);

        }
    }

    public class Validator : AbstractValidator<Query>
    {
        public Validator()
        {
            RuleFor(x => x.TutorEmail).EmailAddress();
            RuleFor(x => x.YearOfDefence).NotEmpty();
            RuleFor(x => x.FieldOfStudyId).GreaterThan(0);
            RuleFor(x => x.Page).GreaterThan(0);
            RuleFor(x => x.ItemsPerPage).GreaterThan(0);
        }
    }
}