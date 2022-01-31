using Application.ExternalServices;
using Auth0.AuthenticationApi;

namespace Infrastructure.Auth0;

public class Auth0UserDataFetcher : IUserDataFetcher
{
    private readonly IAuthenticationApiClient _authenticationApiClient;
    private readonly IContextAccessor _contextAccessor;

    public Auth0UserDataFetcher(IAuthenticationApiClient authenticationApiClient, IContextAccessor contextAccessor)
    {
        _authenticationApiClient =
            authenticationApiClient ?? throw new ArgumentNullException(nameof(authenticationApiClient));
        _contextAccessor = contextAccessor ?? throw new ArgumentNullException(nameof(contextAccessor));
    }

    public async Task<UserDataDto> FetchUserDataAsync(string email)
    {
        var userInfo = await _authenticationApiClient.GetUserInfoAsync(_contextAccessor.AccessToken)
            .ConfigureAwait(false);
        return new UserDataDto(userInfo.FirstName, userInfo.LastName, userInfo.FullName);
    }
}