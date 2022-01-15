using Application.ExternalServices;
using WebApi.Helpers;

namespace WebApi.Services;

public class HttpContextAccessor : IContextAccessor
{
    public HttpContextAccessor(IHttpContextAccessor httpContextAccessor)
    {
        if (httpContextAccessor == null) throw new ArgumentNullException(nameof(httpContextAccessor));
        AccessToken = httpContextAccessor.HttpContext?.User.FindFirst(x =>
                string.Equals(x.Type, ClaimsConstants.AccessTokenClaimType, StringComparison.OrdinalIgnoreCase))!
            .Value!;
    }

    public string AccessToken { get; }
}