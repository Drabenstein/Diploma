using Application.Queries.Dtos;
using Core;
using Dapper;
using FluentValidation;
using MediatR;

namespace Application.Queries;

public static class GetSupervisedTopics
{
    public class Query : IRequest<IEnumerable<FieldOfStudyInitialTableDto<SupervisedThesisDto>>>
    {
        public long Id { get; init; }
    }

    public class Handler : IRequestHandler<Query, IEnumerable<FieldOfStudyInitialTableDto<SupervisedThesisDto>>>
    {
        private const string Query = "SELECT * FROM Thesis;";
        
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        public Handler(ISqlConnectionFactory sqlConnectionFactory)
        {
            _sqlConnectionFactory = sqlConnectionFactory ?? throw new ArgumentNullException(nameof(sqlConnectionFactory));
        }

        public async Task<IEnumerable<FieldOfStudyInitialTableDto<SupervisedThesisDto>>> Handle(Query request, CancellationToken cancellationToken)
        {
            using (var db = await _sqlConnectionFactory.CreateOpenConnectionAsync().ConfigureAwait(false))
            {
                // ...
            }

            return Array.Empty<FieldOfStudyInitialTableDto<SupervisedThesisDto>>();
        }
    }

    public class Validator : AbstractValidator<Query>
    {
        public Validator()
        {
            RuleFor(x => x.Id).GreaterThan(0);
        }
    }
}