﻿using Application.Common;
using Application.Queries.Dtos;
using Core;
using Dapper;
using FluentValidation;
using MediatR;

namespace Application.Queries;

public static class GetAllTopicsForTutorForFieldOfStudyAndYear
{
    public record Query
    (string Email, long FieldOfStudyId, string YearOfDefence, int Page,
        int ItemsPerPage) : IPagedRequest<TutorsTopicDto>;
    public class Handler : IRequestHandler<Query, PagedResultDto<TutorsTopicDto>>
    {
        private const string SqlQuery =
            "select t.topic_id as \"Id\", t.\"name\" as \"Name\", t.english_name as \"EnglishName\", realizer.first_name || ' ' || realizer.last_name || ' ' || realizer.index_number as \"StudentName\", reviewer.academic_degree || ' ' || reviewer.first_name || ' ' || reviewer.last_name as \"ReviewerName\", t.is_free as \"Free\", supervisor.department as \"Department\"" +
                "from topic as t join \"user\" as supervisor on t.supervisor_id = supervisor.user_id" +
                @"join thesis as the on t.topic_id = the.topic_id
                join review as rev on rev.thesis_id = the.thesis_id" +
                "join \"user\" as reviewer on reviewer.user_id = rev.reviewer_id join \"user\" as realizer on realizer.user_id = the.realizer_student_id where reviewer.user_id != supervisor.user_id and t.year_of_defence = :YearOfDefence and t.field_of_study_id = :FieldOfStudyId and t.supervisor_id = :TutorId    ORDER BY realizer.last_name asc, realizer.first_name asc OFFSET :OffsetRows ROWS FETCH NEXT :ItemsPerPage ROWS ONLY";

        private const string SqlCountQuery = @"SELECT COUNT(*)" +
             "from topic as t join \"user\" as supervisor on t.supervisor_id = supervisor.user_id" +
                @"join thesis as the on t.topic_id = the.topic_id
                join review as rev on rev.thesis_id = the.thesis_id" +
                "join \"user\" as reviewer on reviewer.user_id = rev.reviewer_id join \"user\" as realizer on realizer.user_id = the.realizer_student_id where reviewer.user_id != supervisor.user_id and t.year_of_defence = :YearOfDefence and t.field_of_study_id = :FieldOfStudyId and t.supervisor_id = :TutorId";
        
        private const string TutorIdSql = "SELECT user_id FROM \"user\" WHERE email = :Email";

        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        public Handler(ISqlConnectionFactory sqlConnectionFactory)
        {
            _sqlConnectionFactory =
                sqlConnectionFactory ?? throw new ArgumentNullException(nameof(sqlConnectionFactory));
        }

        public async Task<PagedResultDto<TutorsTopicDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            using var connection = await _sqlConnectionFactory.CreateOpenConnectionAsync().ConfigureAwait(false);
            
            var tutorId = await connection.QuerySingleAsync<long>(TutorIdSql).ConfigureAwait(false);

            var results =
                await connection
                    .QueryAsync<TutorsTopicDto>(SqlQuery, new
                    {
                        YearOfDefence = request.YearOfDefence,
                        FieldOfStudyId = request.FieldOfStudyId,
                        OffsetRows = (request.Page - 1) * request.ItemsPerPage,
                        ItemsPerPage = request.ItemsPerPage,
                        TutorId = tutorId
                    }).ConfigureAwait(false);

            return await results.GetPagedResultAsync(connection, SqlCountQuery, new
            {
                YearOfDefence = request.YearOfDefence,
                FieldOfStudyId = request.FieldOfStudyId,
                TutorId = tutorId
            }, request.Page, request.ItemsPerPage).ConfigureAwait(false);
        }
    }

    public class Validator : AbstractValidator<Query>
    {
        public Validator()
        {
            RuleFor(x => x.Email).EmailAddress();
            RuleFor(x => x.YearOfDefence).NotEmpty();
            RuleFor(x => x.FieldOfStudyId).GreaterThan(0);
            RuleFor(x => x.Page).GreaterThan(0);
            RuleFor(x => x.ItemsPerPage).GreaterThan(0);
        }
    }


}