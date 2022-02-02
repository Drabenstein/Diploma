using Application.Common;
using Application.Queries.Dtos;
using Core;
using Dapper;
using FluentValidation;
using MediatR;

namespace Application.Queries;
public static class GetStudentsApprovedTopicsForFieldAndYearAsync
{
    public record Query(string UserEmail, long FieldOfStudyId, string YearOfDefence, int Page, int ItemsPerPage)
        :IRequest<PagedResultDto<StudentsApprovedTopicDto>>;

    public class Handler : IRequestHandler<Query, PagedResultDto<StudentsApprovedTopicDto>>
    {
        private const string SqlQuery = @"
            SELECT t.topic_id as TopicId,
                a.application_id as ApplicationId,
                t.name as Name,
                t.english_name as EnglishName,
                a.status as Status,
                s.academic_degree as SupervisorAcademicDegree,
                s.first_name || ' ' || s.last_name as SupervisorFullName,
                s.department as SupervisorDepartment
            FROM topic t
                JOIN application a ON a.topic_id = t.topic_id
                JOIN " + "\"user\"" + @" s ON t.supervisor_id = s.user_id
            WHERE a.submitter_id = :UserId
                AND a.status IN ('Approved', 'Confirmed')
                AND t.year_of_defence = :YearOfDefence
                AND t.field_of_study_id = :FieldOfStudyId
            OFFSET :OffsetRows ROWS FETCH NEXT :ItemsPerPage ROWS ONLY";

        private const string CountQuery = @"
            SELECT COUNT(*)
            FROM topic t
                JOIN application a ON a.topic_id = t.topic_id
                JOIN " + "\"user\"" + @" s ON t.supervisor_id = s.user_id
            WHERE a.submitter_id = :UserId
                AND a.status IN ('Approved', 'Confirmed')
                AND t.year_of_defence = :YearOfDefence
                AND t.field_of_study_id = :FieldOfStudyId
            ";

        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        public Handler(ISqlConnectionFactory sqlConnectionFactory)
        {
            _sqlConnectionFactory =
                sqlConnectionFactory ?? throw new ArgumentNullException(nameof(sqlConnectionFactory));
        }

        public async Task<PagedResultDto<StudentsApprovedTopicDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            using var connection = await _sqlConnectionFactory.CreateOpenConnectionAsync().ConfigureAwait(false);
            var userId = await connection.GetUserIdByEmailAsync(request.UserEmail).ConfigureAwait(false);

            var results = await connection.QueryAsync<StudentsApprovedTopicDto>(SqlQuery, new
            {
                UserId = userId,
                YearOfDefence = request.YearOfDefence,
                FieldOfStudyId = request.FieldOfStudyId,
                OffsetRows = (request.Page - 1) * request.ItemsPerPage,
                ItemsPerPage = request.ItemsPerPage
            }).ConfigureAwait(false);

            var topics = results.ToArray();
            foreach(var topic in topics)
            {
                topic.SupervisorFullName = topic.SupervisorFullName.CombineAcademicDegreeAndFullName(topic.SupervisorAcademicDegree);
            }

            return await topics.GetPagedResultAsync(connection, CountQuery, new
            {
                UserId = userId,
                YearOfDefence = request.YearOfDefence,
                FieldOfStudyId = request.FieldOfStudyId
            }, request.Page, request.ItemsPerPage).ConfigureAwait(false);

        }

    }

    public class Validator : AbstractValidator<Query>
    {
        public Validator()
        {
            RuleFor(x => x.UserEmail).EmailAddress();
            RuleFor(x => x.FieldOfStudyId).GreaterThan(0);
            RuleFor(x => x.YearOfDefence).NotEmpty();
            RuleFor(x => x.Page).GreaterThan(0);
            RuleFor(x => x.ItemsPerPage).GreaterThan(0);
        }
    }

}
