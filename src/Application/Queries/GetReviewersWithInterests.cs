using Application.Common;
using Application.Queries.Dtos;
using Core;
using Dapper;
using FluentValidation;
using MediatR;

namespace Application.Queries;
public static class GetReviewersWithInterests
{
    public record Query(int MinNumberOfReviews, int MaxNumberOfReviews,
        long[] AreaOfInterestIds, int Page, int ItemsPerPage) : IPagedRequest<ReviewerWithInterestsDto>;

    public class Handler : IRequestHandler<Query, PagedResultDto<ReviewerWithInterestsDto>>
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        public Handler(ISqlConnectionFactory sqlConnectionFactory)
        {
            _sqlConnectionFactory =
                sqlConnectionFactory ?? throw new ArgumentNullException(nameof(sqlConnectionFactory));
        }

        private const string Sql = "select u.first_name || ' ' || u.last_name as \"Employee\", u.department as \"Department\", u.academic_degree as \"Title\", u.\"position\" as \"Position\", count(r.review_id) as \"ReviewsNumber\", string_agg(aoi.areas_of_interest_id::text, ',') as \"AreasOfInterestIds\" from \"user\" as u join area_of_interest_user as aoi on aoi.users_id = u.user_id" +
            @"join review as r on u.user_id = r.reviewer_id
            where aoi.areas_of_interest_id in :AreasOfInterest
            group by u.user_id
                having count(r.review_id) >= :MinNumberOfReviews
                and count(r.review_id) <= :MaxNumberOfReviews
            order by u.department offset :OffsetRows rows fetch next :ItemsPerPage rows only";


        private const string SqlCount = "select count(*) from \"user\" as u join area_of_interest_user as aoi on aoi.users_id = u.user_id" +
           @"join review as r on u.user_id = r.reviewer_id
                where aoi.areas_of_interest_id in :AreasOfInterest
                group by u.user_id
                    having count(r.review_id) >= :MinNumberOfReviews
                    and count(r.review_id) <= :MaxNumberOfReviews";

        public async Task<PagedResultDto<ReviewerWithInterestsDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            using var connection = await _sqlConnectionFactory.CreateOpenConnectionAsync().ConfigureAwait(false);

            var results = await connection.QueryAsync<ReviewerWithInterestsStringDto>(Sql, new
            {
                AreasOfInterest = $"({string.Join(',', request.AreaOfInterestIds)})",
                MinNumberOfReviews = request.MinNumberOfReviews,
                MaxNumberOfReviews = request.MaxNumberOfReviews,
                OffsetRows = (request.Page - 1) * request.ItemsPerPage,
                ItemsPerPage = request.ItemsPerPage
            }).ConfigureAwait(false);

            var dtos = results.Select(x => new ReviewerWithInterestsDto
            {
                Title = x.Title,
                Employee = x.Employee,
                Department = x.Department,
                Position = x.Position,
                ReviewsNumber = x.ReviewsNumber,
                AreasOfInterestIds = x.AreasOfInterestIds.Split(',').Select(long.Parse).ToArray()
            });

            return await dtos.GetPagedResultAsync(connection, SqlCount, new
            {
                AreasOfInterest = $"({string.Join(',', request.AreaOfInterestIds)})",
                MinNumberOfReviews = request.MinNumberOfReviews,
                MaxNumberOfReviews = request.MaxNumberOfReviews
            }, request.Page, request.ItemsPerPage).ConfigureAwait(false);

        }

        private record ReviewerWithInterestsStringDto(string Title, string Employee, string Department, string Position, int ReviewsNumber, string AreasOfInterestIds);

        public class Validator : AbstractValidator<Query>
        {
            public Validator()
            {
                RuleFor(x => x.MinNumberOfReviews).GreaterThanOrEqualTo(0).LessThanOrEqualTo(x => x.MaxNumberOfReviews);
                RuleFor(x => x.MaxNumberOfReviews).GreaterThanOrEqualTo(0);
                RuleForEach(x => x.AreaOfInterestIds).GreaterThan(0);
            }
        }

    }
}
