﻿using Application.Queries.Dtos;
using Core;
using Dapper;
using MediatR;

namespace Application.Queries;
public static class GetAllTopicsForTutor
{
    public record Query(string Email) : IRequest<IEnumerable<FieldOfStudyInitialTableDto<TutorsTopicDto>>>;

    public class Handler : IRequestHandler<Query, IEnumerable<FieldOfStudyInitialTableDto<TutorsTopicDto>>>
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

        public async Task<IEnumerable<FieldOfStudyInitialTableDto<TutorsTopicDto>>> Handle(Query request, CancellationToken cancellationToken)
        {
            using var connection = await _sqlConnectionFactory.CreateOpenConnectionAsync().ConfigureAwait(false);
            var initialTableDtos = await connection.QueryAsync<FieldOfStudyInitialTableDto<TutorsTopicDto>>(Sql).ConfigureAwait(false);

            var fieldOfStudyInitialTableDtos = initialTableDtos as FieldOfStudyInitialTableDto<TutorsTopicDto>[] ??
                                   initialTableDtos.ToArray();

            foreach (var initialTableDto in fieldOfStudyInitialTableDtos)
            {
                initialTableDto.Data = await _mediator
                    .Send(CreateDataQuery(request.Email, initialTableDto.DefenceYear, initialTableDto.Id),
                        cancellationToken).ConfigureAwait(false);
            }

            return fieldOfStudyInitialTableDtos;
        }

        private GetAllTopicsForTutorForFieldOfStudyAndYear.Query CreateDataQuery(string email, string defenceYear, long fieldOfStudyId)
        {
            const int defaultPage = 1;
            const int defaultItemsPerPage = 10;
            return new GetAllTopicsForTutorForFieldOfStudyAndYear.Query(Email: email,
                FieldOfStudyId: fieldOfStudyId, YearOfDefence: defenceYear, Page: defaultPage,
                ItemsPerPage: defaultItemsPerPage);
        }

    }
}

