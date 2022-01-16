using Application.Common;
using Application.Queries.Dtos;
using Core;
using Dapper;
using FluentValidation;
using MediatR;

namespace Application.Queries;

public static class GetApplicationDetailsById
{
    public record Query(string UserEmail, long ApplicationId) : IRequest<ApplicationDetailsDto?>;

    public class Handler : IRequestHandler<Query, ApplicationDetailsDto?>
    {
        private const string SqlQuery = @"SELECT
        s.first_name || ' ' || s.last_name AS StudentFullName,
        s.index_number AS StudentIndex,
        fos.name AS FieldOfStudyName,
        fos.degree AS FieldOfStudyDegree,
        fos.study_form AS StudyForm,
        t.year_of_defence AS YearOfDefence,
        t.max_realization_number AS TopicMaxRealizationNumber,
        (SELECT COUNT(*) FROM application a1 WHERE a1.topic_id = t.topic_id AND a1.status IN ('Accepted', 'Confirmed')) AS TopicCurrentRealizations,
            t.name AS TopicName,
        t.english_name AS TopicEnglishName,
        a.message AS Message
            FROM application a
        JOIN topic t
            ON a.topic_id = t.topic_id
            JOIN " + "\"user\"" + @" s
            ON a.submitter_id = s.user_id
            JOIN field_of_study fos
        ON t.field_of_study_id = fos.field_of_study_id
            WHERE a.application_id = :ApplicationId
                AND (a.submitter_id = :UserId OR t.supervisor_id = :UserId)";
        
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        public Handler(ISqlConnectionFactory sqlConnectionFactory)
        {
            _sqlConnectionFactory = sqlConnectionFactory ?? throw new ArgumentNullException(nameof(sqlConnectionFactory));
        }

        public async Task<ApplicationDetailsDto?> Handle(Query request, CancellationToken cancellationToken)
        {
            using var connection = await _sqlConnectionFactory.CreateOpenConnectionAsync().ConfigureAwait(false);
            var userId = await connection.GetUserIdByEmailAsync(request.UserEmail).ConfigureAwait(false);
            var result = await connection.QueryFirstOrDefaultAsync(SqlQuery, new
            {
                ApplicationId = request.ApplicationId,
                UserId = userId
            }).ConfigureAwait(false);
            return result;
        }
    }

    public class Validator : AbstractValidator<Query>
    {
        public Validator()
        {
            RuleFor(x => x.ApplicationId).GreaterThan(0);
            RuleFor(x => x.UserEmail).EmailAddress();
        }
    }
}