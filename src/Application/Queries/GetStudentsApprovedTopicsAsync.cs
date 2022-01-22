using Application.Common;
using Application.Queries.Dtos;
using Core;
using Dapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries;
public static class GetStudentsApprovedTopicsAsync
{
    public record Query(string UserEmail) : IRequest<IEnumerable<FieldOfStudyInitialTableDto<StudentsApprovedTopicDto>>>;

    public class Handler : IRequestHandler<Query,
        IEnumerable<FieldOfStudyInitialTableDto<StudentsApprovedTopicDto>>>
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
                    JOIN field_of_study fos ON t.field_of_study_id = fos.field_of_study_id
                    JOIN student_field_of_study sfos on sfos.field_of_study_id = fos.field_of_study_id 
                    WHERE sfos.student_id = :UserId
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

        public async Task<IEnumerable<FieldOfStudyInitialTableDto<StudentsApprovedTopicDto>>> Handle(Query request,
            CancellationToken cancellationToken)
        {
            using var connection = await _sqlConnectionFactory.CreateOpenConnectionAsync().ConfigureAwait(false);
            var userId = await connection.GetUserIdByEmailAsync(request.UserEmail).ConfigureAwait(false);

            var fositDtos = await connection
                .QueryAsync<FieldOfStudyInitialTableDto<StudentsApprovedTopicDto>>(
                    SqlQuery, new { UserId = userId }).ConfigureAwait(false);

            const int defaultPage = 1;
            const int defaultItemsPerPage = 10;
            var initialTableDtos = fositDtos as FieldOfStudyInitialTableDto<StudentsApprovedTopicDto>[] ??
                                   fositDtos.ToArray();
            foreach (var fosit in initialTableDtos)
            {
                fosit.Data = await _mediator.Send(new GetStudentsApprovedTopicsForFieldAndYearAsync.Query(
                        request.UserEmail,
                        fosit.Id,
                        fosit.DefenceYear, defaultPage, defaultItemsPerPage), cancellationToken)
                    .ConfigureAwait(false);
            }

            return initialTableDtos;
        }
    }
}

