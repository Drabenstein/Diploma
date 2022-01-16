using Application.Queries.Dtos;
using Core;
using Dapper;
using FluentValidation;
using MediatR;

namespace Application.Queries;
public static class GetStudentData
{
    public record Query(string email) : IRequest<IEnumerable<StudentDataDto>>;

    public class Handler : IRequestHandler<Query, IEnumerable<StudentDataDto>>
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        public Handler(ISqlConnectionFactory sqlConnectionFactory)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
        }

        private const string Sql =
            "select u.first_name || ' ' || u.last_name as \"Name\", u.index_number as \"Index\", fos.\"name\" as \"FieldOfStudy\", sfos.specialization as \"Specialization\", fos.\"degree\" as \"Degree\", sfos.semester as \"Semester\", fos.study_form as \"StudyForm\" from \"user\" as u join student_field_of_study sfos on u.user_id = sfos.student_id join field_of_study fos on sfos.field_of_study_id = fos.field_of_study_id where u.email = :Email";

        public async Task<IEnumerable<StudentDataDto>> Handle(Query query, CancellationToken cancellationToken)
        {
            using var connection = await _sqlConnectionFactory.CreateOpenConnectionAsync().ConfigureAwait(false);
            return await connection.QueryAsync<StudentDataDto>(Sql, new { 
                Email = query.email
            }).ConfigureAwait(false);
        }
    }

    public class Validator : AbstractValidator<Query>
    {
        public Validator()
        {
            RuleFor(x => x.email).EmailAddress();
        }
    }
}
