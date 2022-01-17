using Application.Common;
using Application.Queries.Dtos;
using Core;
using Dapper;
using FluentValidation;
using MediatR;

namespace Application.Queries;

public static class GetSupervisedTheses
{
    public record Query(string TutorEmail) : IRequest<IEnumerable<FieldOfStudyInitialTableDto<SupervisedThesisDto>>>;

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

        public async Task<IEnumerable<FieldOfStudyInitialTableDto<SupervisedThesisDto>>> Handle(Query request,
            CancellationToken cancellationToken)
        {
            using var connection = await _sqlConnectionFactory.CreateOpenConnectionAsync().ConfigureAwait(false);

            var tutorId = await connection.GetUserIdByEmailAsync(request.TutorEmail).ConfigureAwait(false);
            
            var initialTableDtos = await connection.QueryAsync<FieldOfStudyInitialTableDto<SupervisedThesisDto>>(
                SqlQuery,
                new
                {
                    TutorId = tutorId
                }).ConfigureAwait(false);

            var fieldOfStudyInitialTableDtos = initialTableDtos as FieldOfStudyInitialTableDto<SupervisedThesisDto>[] ??
                                               initialTableDtos.ToArray();

            foreach (var initialTableDto in fieldOfStudyInitialTableDtos)
            {
                initialTableDto.Data = await _mediator
                    .Send(CreateDataQuery(request.TutorEmail, initialTableDto.DefenceYear, initialTableDto.Id),
                        cancellationToken).ConfigureAwait(false);
            }

            return fieldOfStudyInitialTableDtos;
        }

        private GetSupervisedThesesForYearOfDefenceAndField.Query CreateDataQuery(string tutorEmail, string defenceYear,
            long fieldOfStudyId)
        {
            const int defaultPage = 0;
            const int defaultItemsPerPage = 10;
            return new GetSupervisedThesesForYearOfDefenceAndField.Query(TutorEmail: tutorEmail,
                FieldOfStudyId: fieldOfStudyId, YearOfDefence: defenceYear, Page: defaultPage,
                ItemsPerPage: defaultItemsPerPage);
        }
    }

    public class Validator : AbstractValidator<Query>
    {
        public Validator()
        {
            RuleFor(x => x.TutorEmail).EmailAddress();
        }
    }
}