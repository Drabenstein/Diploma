﻿using Microsoft.AspNetCore.Mvc;
using System.Collections.Immutable;
using System.Security.Claims;
using WebApi.Helpers;

namespace WebApi.Common;

[ApiController]
public abstract class BaseApiController : ControllerBase
{
    protected const int DefaultPage = 1;
    protected const int DefaultPageSize = 10;

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

    protected string GetUserId()
    {
        return User.Claims.FirstOrDefault(x => 
                    x.Type.Equals(ClaimTypes.NameIdentifier, StringComparison.OrdinalIgnoreCase))?.Value ?? 
               throw new Exception("Logged-in user id not found");
    }
}