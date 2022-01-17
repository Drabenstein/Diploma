using Application.Queries.Dtos;
using Core;
using Dapper;
using MediatR;

namespace Application.Queries;
public static class GetAllTopics
{
    public record Query() : IRequest<IEnumerable<FieldOfStudyInitialTableDto<StudentsTopicDto>>>;

    public class Handler : IRequestHandler<Query, IEnumerable<FieldOfStudyInitialTableDto<StudentsTopicDto>>>
    {
        private const string Sql = @"SELECT fos.field_of_study_id AS Id,
            fos.name AS Name,
            fos.degree AS Degree,
            fos.study_form AS StudyForm,
            fos.lecture_language AS LectureLanguage,
            t.year_of_defence AS DefenceYear
            FROM topic t
                JOIN field_of_study fos on t.field_of_study_id = fos.field_of_study_id
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

        public async Task<IEnumerable<FieldOfStudyInitialTableDto<StudentsTopicDto>>> Handle(Query request, CancellationToken cancellationToken)
        {
            using var connection = await _sqlConnectionFactory.CreateOpenConnectionAsync().ConfigureAwait(false);
            var initialTableDtos = await connection.QueryAsync<FieldOfStudyInitialTableDto<StudentsTopicDto>>(Sql).ConfigureAwait(false);

            var fieldOfStudyInitialTableDtos = initialTableDtos as FieldOfStudyInitialTableDto<StudentsTopicDto>[] ??
                                   initialTableDtos.ToArray();

            foreach (var initialTableDto in fieldOfStudyInitialTableDtos)
            {
                initialTableDto.Data = await _mediator
                    .Send(CreateDataQuery(initialTableDto.DefenceYear, initialTableDto.Id),
                        cancellationToken).ConfigureAwait(false);
            }

            return fieldOfStudyInitialTableDtos;
        }

        private GetAllTopicsForFieldOfStudyAndYear.Query CreateDataQuery(string defenceYear, long fieldOfStudyId)
        {
            const int defaultPage = 0;
            const int defaultItemsPerPage = 10;
            return new GetAllTopicsForFieldOfStudyAndYear.Query(
                FieldOfStudyId: fieldOfStudyId, YearOfDefence: defenceYear, Page: defaultPage,
                ItemsPerPage: defaultItemsPerPage);
        }

    }
}

