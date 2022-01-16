using System.Data;
using Dapper;

namespace Application.Common;

public static class PagedQueryExtensions
{
    private const string TutorIdQuery = "SELECT u.user_id FROM \"user\" u WHERE u.email = :UserEmail";

    public static Task<long> GetUserIdByEmailAsync(this IDbConnection connection, string userEmail)
    {
        return connection.QuerySingleAsync<long>(TutorIdQuery, new
        {
            UserEmail = userEmail
        });
    }

    public static async Task<PagedResultDto<T>> GetPagedResultAsync<T>(this IEnumerable<T> results,
        IDbConnection connection, string countQuery,
        object countQueryParams,
        int page, int itemsPerPage)
    {
        var totalCount = await connection.ExecuteScalarAsync<int>(countQuery, countQueryParams)
            .ConfigureAwait(false);

        var hasNextPage = totalCount / itemsPerPage > page;

        return new PagedResultDto<T>
        {
            CurrentPage = page,
            TotalItems = totalCount,
            HasNextPage = hasNextPage,
            Results = results
        };
    }
}