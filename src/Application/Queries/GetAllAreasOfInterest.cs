using Application.Common;
using Core;
using Dapper;
using MediatR;

public static class GetAllAreasOfInterest
{
    public record Query() : IRequest<IEnumerable<AreaOfInterestDto>>;

    public class Handler : IRequestHandler<Query, IEnumerable<AreaOfInterestDto>>
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        public Handler(ISqlConnectionFactory sqlConnectionFactory)
        {
            _sqlConnectionFactory = sqlConnectionFactory ?? throw new ArgumentNullException(nameof(sqlConnectionFactory));
        }

        private const string Sql = "select area_of_interest_id as \"Id\", \"name\" as \"Name\" from area_of_interest";

        public async Task <IEnumerable<AreaOfInterestDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            using var connection = await _sqlConnectionFactory.CreateOpenConnectionAsync().ConfigureAwait(false);

            return await connection.QueryAsync<AreaOfInterestDto>(Sql).ConfigureAwait(false);
        }
    }

}
