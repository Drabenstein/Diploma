using Amazon;
using Amazon.Comprehend;
using Amazon.S3;
using Amazon.Translate;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Core;
using Infrastructure.Database;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace WebApi.Helpers;

public static class DependencyInjectionExtensions
{
    public static void AddDatabaseServices(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<DiplomaDbContext>(x => x.UseNpgsql(connectionString,
            b => b.MigrationsAssembly("Infrastructure")));
        services.AddScoped<ISqlConnectionFactory>(_ => new SqlConnectionFactory(connectionString));
    }

    public static void AddAuth0Authentication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

        }).AddJwtBearer(options =>
        {
            options.Authority = configuration["Auth0:Authority"];
            options.Audience = configuration["Auth0:Audience"];
            
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidIssuer = configuration["Auth0:ValidIssuer"],
                ValidateAudience = true,
                ValidateIssuer = true,
            };
            
            options.Events = new JwtBearerEvents
            {
                OnTokenValidated = context =>
                {
                    var accessToken = context.SecurityToken as JwtSecurityToken;
                    if (accessToken is { })
                    {
                        ClaimsIdentity? identity = context.Principal?.Identity as ClaimsIdentity;
                        identity?.AddClaim(new Claim(ClaimsConstants.AccessTokenClaimType, accessToken.RawData));
                    }

                    return Task.CompletedTask;
                }
            };
        });
    }

    public static void AddAmazonClients(this IServiceCollection services)
    {
        services.AddScoped(_ => new AmazonS3Client(RegionEndpoint.USEast1));
        services.AddScoped(_ => new AmazonTranslateClient(RegionEndpoint.USEast1));
        services.AddScoped(_ => new AmazonComprehendClient(RegionEndpoint.USEast1));
    }
}