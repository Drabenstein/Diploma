using Application.ExternalServices;
using Auth0.AuthenticationApi;
using Auth0.AuthenticationApi.Models;
using Auth0.ManagementApi;
using Auth0.ManagementApi.Models;
using Microsoft.Extensions.Options;

namespace Infrastructure.Auth0;

public class Auth0UserService : IUserService
{
    private readonly IOptionsSnapshot<Auth0ApiConfig> _apiConfigSnapshot;

    public Auth0UserService(IOptionsSnapshot<Auth0ApiConfig> apiConfigSnapshot)
    {
        _apiConfigSnapshot = apiConfigSnapshot ?? throw new ArgumentNullException(nameof(apiConfigSnapshot));
    }

    public async Task ChangePasswordAsync(string userExternalId, string password)
    {
        var accessToken = await GetApiTokenAsync().ConfigureAwait(false);
        var managementClient = new ManagementApiClient(accessToken, _apiConfigSnapshot.Value.Auth0Domain);
        var passwordChangeRequest = new UserUpdateRequest
        {
            Password = password
        };
        await managementClient.Users.UpdateAsync(userExternalId, passwordChangeRequest).ConfigureAwait(false);
    }

    private async Task<string> GetApiTokenAsync()
    {
        var authenticationClient = new AuthenticationApiClient(_apiConfigSnapshot.Value.Auth0Domain);
        var tokenRequest = new ClientCredentialsTokenRequest
        {
            ClientId = _apiConfigSnapshot.Value.ClientId,
            ClientSecret = _apiConfigSnapshot.Value.ClientSecret,
            Audience = _apiConfigSnapshot.Value.Audience
        };
        var tokenReponse = await authenticationClient.GetTokenAsync(tokenRequest).ConfigureAwait(false);

        if (tokenReponse is null)
        {
            throw new ExternalServiceFailureException("Could not login to Auth0 API");
        }

        return tokenReponse.AccessToken;
    }
}
