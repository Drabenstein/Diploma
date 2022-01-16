using System.Collections.Immutable;
using Microsoft.AspNetCore.Mvc;
using WebApi.Helpers;

namespace WebApi.Common;

[ApiController]
public abstract class BaseApiController : ControllerBase
{
    protected IReadOnlyCollection<string> GetUserRoles()
    {
        return User.Claims
            .Where(x => string.Equals(x.Type, ClaimsConstants.RoleClaimType, StringComparison.Ordinal))
            .Select(x => x.Value)
            .ToImmutableList();
    }

    protected string GetUserEmail()
    {
        return User.Claims.First(x =>
                   string.Equals(x.Type, ClaimsConstants.EmailClaimType, StringComparison.OrdinalIgnoreCase))?.Value ??
               throw new Exception("Logged-in user e-mail not found");
    }
}