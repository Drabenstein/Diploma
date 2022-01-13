using Application.Common;
using Application.Queries.Dtos;
using Core;
using Dapper;
using FluentValidation;
using MediatR;

namespace Application.Queries;

public static class GetSupervisedThesesForYearOfDefenceAndField
{
    public record Query
    (long TutorId, long FieldOfStudyId, string YearOfDefence, long Page,
        long ItemsPerPage) : IPagedRequest<SupervisedThesisDto>;

    public class Handler : IRequestHandler<Query, PagedResultDto<SupervisedThesisDto>>
    {
        private const string SqlQuery =
            "SELECT t.thesis_id AS Id, t2.name AS Name, t2.english_name AS EnglishName, supervisor.academic_degree AS SupervisorAcademicDegree, supervisor.first_name + ' ' + supervisor.last_name AS SupervisorFullName, reviewer.academic_degree AS ReviewerAcademicDegree, reviewer.first_name + ' ' + reviewer.last_name AS ReviewerFullName, student.first_name + ' ' + student.last_name AS StudentFullName FROM thesis t JOIN topic t2 ON t.topic_id = t2.topic_id JOIN field_of_study fos ON t2.field_of_study_id = fos.field_of_study_id JOIN \"user\" student ON t.realizer_student_id = student.user_id JOIN \"user\" supervisor ON supervisor.user_id = t2.supervisor_id LEFT JOIN review r ON t.thesis_id = r.thesis_id LEFT JOIN \"user\" reviewer ON reviewer.user_id = r.reviewer_id WHERE t2.supervisor_id = :TutorId AND t2.year_of_defence = :YearOfDefence AND fos.field_of_study_id = :FieldOfStudyId OFFSET :Offset ROWS FETCH NEXT :ItemsPerPage ROWS ONLY";

        private const string SqlCountQuery = @"SELECT COUNT(*)
            FROM thesis t
                JOIN topic t2 ON t.topic_id = t2.topic_id
                JOIN field_of_study fos ON t2.field_of_study_id = fos.field_of_study_id
            WHERE t2.supervisor_id = :TutorId
                AND t2.year_of_defence = :YearOfDefence
                AND fos.field_of_study_id = :FieldOfStudyId";

        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        public Handler(ISqlConnectionFactory sqlConnectionFactory)
        {
            _sqlConnectionFactory =
                sqlConnectionFactory ?? throw new ArgumentNullException(nameof(sqlConnectionFactory));
        }

        public async Task<PagedResultDto<SupervisedThesisDto>> Handle(Query request,
            CancellationToken cancellationToken)
        {
            using var connection = await _sqlConnectionFactory.CreateOpenConnectionAsync().ConfigureAwait(false);
            var results =
                await connection
                    .QueryAsync<SupervisedThesisDto>(SqlQuery, new
                    {
                        request.TutorId,
                        request.FieldOfStudyId,
                        request.YearOfDefence,
                        Offset = (request.Page - 1) * request.ItemsPerPage,
                        request.ItemsPerPage
                    }).ConfigureAwait(false);

            var totalCount = await connection.ExecuteScalarAsync<int>(SqlCountQuery, new
                {
                    request.TutorId,
                    request.FieldOfStudyId,
                    request.YearOfDefence,
                })
                .ConfigureAwait(false);

            var hasNextPage = (totalCount / request.ItemsPerPage) > request.Page;

            return new PagedResultDto<SupervisedThesisDto>()
            {
                CurrentPage = request.Page,
                TotalItems = totalCount,
                HasNextPage = hasNextPage,
                Results = results
            };
        }
    }

    public class Validator : AbstractValidator<Query>
    {
        public Validator()
        {
            RuleFor(x => x.TutorId).GreaterThan(0);
            RuleFor(x => x.Page).GreaterThanOrEqualTo(0);
            RuleFor(x => x.ItemsPerPage).GreaterThan(0);
            RuleFor(x => x.YearOfDefence).NotEmpty();
            RuleFor(x => x.FieldOfStudyId).GreaterThan(0);
        }
    }
}