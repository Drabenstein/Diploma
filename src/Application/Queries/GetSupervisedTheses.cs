using Application.Queries.Dtos;
using Core;
using Dapper;
using FluentValidation;
using MediatR;

namespace Application.Queries;

public class GetSupervisedTheses
{
    public class Query : IRequest<IEnumerable<FieldOfStudyInitialTableDto<SupervisedThesisDto>>>
    {
        public long TutorId { get; init; }
    }

    public class Handler : IRequestHandler<Query, IEnumerable<FieldOfStudyInitialTableDto<SupervisedThesisDto>>>
    {
        private const string SqlQuery = @"SELECT fos.field_of_study_id AS Id,
        fos.name AS Name,
        fos.degree AS Degree,
        fos.study_form AS StudyForm,
        fos.lecture_language AS LectureLanguage,
        t.year_of_defence AS DefenceYear
            FROM topic t
        JOIN field_of_study fos on t.field_of_study_id = fos.field_of_study_id
            WHERE supervisor_id = :TutorId
            AND EXISTS (SELECT * FROM thesis t2
        WHERE t2.topic_id = t.topic_id)";

        private const int DefaultItemsPerPage = 10;

        private readonly ISqlConnectionFactory _sqlConnectionFactory;
        private readonly IMediator _mediator;

        public Handler(ISqlConnectionFactory sqlConnectionFactory, IMediator mediator)
        {
            _sqlConnectionFactory =
                sqlConnectionFactory ?? throw new ArgumentNullException(nameof(sqlConnectionFactory));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<IEnumerable<FieldOfStudyInitialTableDto<SupervisedThesisDto>>> Handle(Query request,
            CancellationToken cancellationToken)
        {
            using var connection = await _sqlConnectionFactory.CreateOpenConnectionAsync().ConfigureAwait(false);
            var initialTableDtos = await connection.QueryAsync<FieldOfStudyInitialTableDto<SupervisedThesisDto>>(
                SqlQuery,
                new
                {
                    request.TutorId
                }).ConfigureAwait(false);

            var fieldOfStudyInitialTableDtos = initialTableDtos as FieldOfStudyInitialTableDto<SupervisedThesisDto>[] ??
                                               initialTableDtos.ToArray();

            foreach (var initialTableDto in fieldOfStudyInitialTableDtos)
            {
                initialTableDto.Data = await _mediator
                    .Send(CreateDataQuery(request.TutorId, initialTableDto.DefenceYear, initialTableDto.Id),
                        cancellationToken).ConfigureAwait(false);
            }

            return fieldOfStudyInitialTableDtos;
        }

        private GetSupervisedThesesForYearOfDefenceAndField.Query CreateDataQuery(long tutorId, string defenceYear,
            long fieldOfStudyId)
        {
            return new GetSupervisedThesesForYearOfDefenceAndField.Query
            {
                Page = 0,
                TutorId = tutorId,
                ItemsPerPage = DefaultItemsPerPage,
                YearOfDefence = defenceYear,
                FieldOfStudyId = fieldOfStudyId
            };
        }
    }

    public class Validator : AbstractValidator<Query>
    {
        public Validator()
        {
            RuleFor(x => x.TutorId).GreaterThan(0);
        }
    }
}