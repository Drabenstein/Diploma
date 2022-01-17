using Application.Queries.Dtos;
using Core;
using Dapper;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries;
public static  class GetTutorData
{
    public record Query(string email) : IRequest<TutorDataDto>;

    public class Handler : IRequestHandler<Query, TutorDataDto>
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        public Handler(ISqlConnectionFactory sqlConnectionFactory)
        {
            _sqlConnectionFactory = sqlConnectionFactory ?? throw new ArgumentNullException(nameof(sqlConnectionFactory));
        }

        private const string Sql = "select u.first_name || ' ' || u.last_name as \"Name\", u.department as \"Department\", u.\"position\" as \"Position\", u.has_consent_to_extend_pensum as \"HasConsentToExtendPensum\", string_agg(aoi.areas_of_interest_id::text, ',') as \"AreasOfInterestIds\" from \"user\" as u join area_of_interest_user as aoi on aoi.users_id = u.user_id" +
            @"where u.email = :Email
            group by u.user_id";

        public async Task<TutorDataDto> Handle(Query query, CancellationToken cancellationToken)
        {
            using var connection = await _sqlConnectionFactory.CreateOpenConnectionAsync().ConfigureAwait(false);
            var res = await connection.QuerySingleAsync<TutorDataWithStringIds>(Sql, new
            {
                Email = query.email
            }).ConfigureAwait(false);

            return new TutorDataDto
            {
                Name = res.Name,
                Department = res.Department,
                Position = res.Position,
                HasConsentToExtendPensum = res.HasConsentToExtendPensum,
                AreasOfInterestIds = res.AreasOfInterestIds.Split(',').Select(long.Parse).ToArray()
            };


        }

        private record TutorDataWithStringIds(string Name, string Department, string Position,
            bool HasConsentToExtendPensum, string AreasOfInterestIds);
    }

    public class Validator : AbstractValidator<Query>
    {
        public Validator()
        {
            RuleFor(x => x.email).EmailAddress();
        }
    }
}
