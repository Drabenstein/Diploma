using Application.Common;
using Application.Queries.Dtos;
using Core;
using Dapper;
using FluentValidation;
using MediatR;

namespace Application.Queries;

public static class GetThesesForReviewerAssignmentForYearOfDefenceAndField
{
    public record Query
        (long FieldOfStudyId, string YearOfDefence, int Page, int ItemsPerPage) : IRequest<
            PagedResultDto<ThesisForReviewerAssignmentDto>>;

    public class Handler : IRequestHandler<Query, PagedResultDto<ThesisForReviewerAssignmentDto>>
    {
        private const string SqlQuery = @"
                SELECT  t.thesis_id AS Id,
                        top.name AS Name,
                        top.english_name AS EnglishName,
                        supervisor.academic_degree AS SupervisorAcademicDegree,
                        supervisor.first_name || ' ' || supervisor.last_name AS SupervisorFullName,
                        reviewer.academic_degree AS ReviewerAcademicDegree,
                        reviewer.first_name || ' ' || reviewer.last_name AS ReviewerFullName,
                        reviewer.user_id AS ReviewerId
                FROM thesis t
                    JOIN topic top ON t.topic_id = top.topic_id
                    JOIN " + "\"user\"" + @" supervisor ON top.supervisor_id = supervisor.user_id
                    LEFT JOIN review r ON t.thesis_id = r.thesis_id
                    LEFT JOIN " + "\"user\"" + @" reviewer ON r.reviewer_id = reviewer.user_id
                WHERE top.field_of_study_id = :FieldOfStudyId
                    AND top.year_of_defence = :YearOfDefence
                    AND t.status <> 'Reviewed'
                ORDER BY top.year_of_defence DESC, top.field_of_study_id ASC, top.topic_id DESC
                OFFSET :OffsetRows ROWS FETCH NEXT :ItemsPerPage ROWS ONLY";

        private const string CountQuery = @"
                SELECT COUNT(*)
                FROM thesis t
                    JOIN topic top ON t.topic_id = top.topic_id
                WHERE top.field_of_study_id = :FieldOfStudyId
                    AND top.year_of_defence = :YearOfDefence
                    AND t.status <> 'Reviewed'";

        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        public Handler(ISqlConnectionFactory sqlConnectionFactory)
        {
            _sqlConnectionFactory =
                sqlConnectionFactory ?? throw new ArgumentNullException(nameof(sqlConnectionFactory));
        }

        public async Task<PagedResultDto<ThesisForReviewerAssignmentDto>> Handle(Query request,
            CancellationToken cancellationToken)
        {
            using var connection = await _sqlConnectionFactory.CreateOpenConnectionAsync().ConfigureAwait(false);
            var results = await connection.QueryAsync<ThesisForReviewerAssignmentDto>(SqlQuery, new
            {
                FieldOfStudyId = request.FieldOfStudyId,
                YearOfDefence = request.YearOfDefence,
                OffsetRows = (request.Page - 1) * request.ItemsPerPage,
                ItemsPerPage = request.ItemsPerPage
            }).ConfigureAwait(false);

            var topicForReviewerAssignmentDtos = results as ThesisForReviewerAssignmentDto[] ?? results.ToArray();
            foreach (var tfrad in topicForReviewerAssignmentDtos)
            {
                tfrad.SupervisorFullName =
                    tfrad.SupervisorFullName.CombineAcademicDegreeAndFullName(tfrad.SupervisorAcademicDegree);
                tfrad.ReviewerFullName =
                    tfrad.ReviewerFullName.CombineAcademicDegreeAndFullName(tfrad.ReviewerAcademicDegree);
            }

            return await topicForReviewerAssignmentDtos.GetPagedResultAsync(connection, CountQuery, new
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
            RuleFor(x => x.Page).GreaterThan(0);
            RuleFor(x => x.ItemsPerPage).GreaterThan(0);
        }
    }
}