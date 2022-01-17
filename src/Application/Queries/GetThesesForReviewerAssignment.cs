using Application.Queries.Dtos;
using Core;
using Dapper;
using MediatR;

namespace Application.Queries;

public static class GetThesesForReviewerAssignment
{
    public record Query() : IRequest<IEnumerable<FieldOfStudyInitialTableDto<ThesisForReviewerAssignmentDto>>>;

    public class Handler : IRequestHandler<Query,
        IEnumerable<FieldOfStudyInitialTableDto<ThesisForReviewerAssignmentDto>>>
    {
        private const string SqlQuery = @"
                SELECT
                    fos.field_of_study_id AS Id,
                    fos.name AS Name,
                    fos.degree AS Degree,
                    fos.study_form AS StudyForm,
                    fos.lecture_language AS LectureLanguage,
                    t.year_of_defence AS DefenceYear
                FROM topic t
                    JOIN field_of_study fos
                        ON t.field_of_study_id = fos.field_of_study_id
                WHERE EXISTS (
                        SELECT * FROM Thesis th
                        WHERE th.topic_id = t.topic_id
                    AND th.status <> 'Reviewed')
                GROUP BY fos.field_of_study_id, fos.name, fos.degree, fos.study_form, fos.lecture_language, t.year_of_defence
                ORDER BY t.year_of_defence DESC, fos.name ASC";

        private readonly ISqlConnectionFactory _sqlConnectionFactory;
        private readonly IMediator _mediator;

        public Handler(ISqlConnectionFactory sqlConnectionFactory, IMediator mediator)
        {
            _sqlConnectionFactory =
                sqlConnectionFactory ?? throw new ArgumentNullException(nameof(sqlConnectionFactory));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<IEnumerable<FieldOfStudyInitialTableDto<ThesisForReviewerAssignmentDto>>> Handle(Query request,
            CancellationToken cancellationToken)
        {
            using var connection = await _sqlConnectionFactory.CreateOpenConnectionAsync().ConfigureAwait(false);
            var fositDtos = await connection
                .QueryAsync<FieldOfStudyInitialTableDto<ThesisForReviewerAssignmentDto>>(
                    SqlQuery).ConfigureAwait(false);

            const int defaultPage = 0;
            const int defaultItemsPerPage = 10;
            var initialTableDtos = fositDtos as FieldOfStudyInitialTableDto<ThesisForReviewerAssignmentDto>[] ??
                                   fositDtos.ToArray();
            foreach (var fosit in initialTableDtos)
            {
                fosit.Data = await _mediator.Send(new GetThesesForReviewerAssignmentForYearOfDefenceAndField.Query(
                        fosit.Id,
                        fosit.DefenceYear, defaultPage, defaultItemsPerPage), cancellationToken)
                    .ConfigureAwait(false);
            }

            return initialTableDtos;
        }
    }
}