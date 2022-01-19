using Amazon;
using Amazon.Comprehend;
using Amazon.S3;
using Amazon.Translate;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Application.Common;
using Application.ExternalServices;
using Auth0.AuthenticationApi;
using Core;
using FluentValidation;
using Infrastructure.Auth0;
using Infrastructure.Database;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using HttpContextAccessor = WebApi.Services.HttpContextAccessor;
using Application.Amazon;
using Infrastructure.AWS;

namespace WebApi.Helpers;

public static class DependencyInjectionExtensions
{
    public static void AddDatabaseServices(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<DiplomaDbContext>(x => x.UseNpgsql(connectionString,
            b => b.MigrationsAssembly("Infrastructure")));
        services.AddScoped<ISqlConnectionFactory>(_ => new SqlConnectionFactory(connectionString));
        services.AddScoped<DbContext, DiplomaDbContext>();
    }

    public static void AddCaching(this IServiceCollection services)
    {
        services.AddMemoryCache();
    }

    public static void AddCommandQueries(this IServiceCollection services)
    {
        services.AddMediatR(typeof(PagedResultDto<>).Assembly);
        services.AddValidatorsFromAssembly(typeof(PagedResultDto<>).Assembly);
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
    }
    
    public static void AddHttpContextServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpContextAccessor();
        services.AddScoped<IContextAccessor, HttpContextAccessor>();
        services.AddScoped<IUserDataFetcher, Auth0UserDataFetcher>();
        services.AddScoped<IAuthenticationApiClient>(_ => new AuthenticationApiClient(configuration["Auth0:Domain"]));
    }
    
    public static void AddAuth0Authentication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.Authority = configuration["Auth0:Authority"];
                options.Audience = configuration["Auth0:Audience"];

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    NameClaimType = ClaimTypes.NameIdentifier,
                    ValidIssuer = configuration["Auth0:ValidIssuer"],
                    ValidateAudience = true,
                    ValidateIssuer = true,
                    RoleClaimType = ClaimsConstants.RoleClaimType
                };

                options.Events = new JwtBearerEvents
                {
                    OnTokenValidated = context =>
                    {
                        if (context.SecurityToken is JwtSecurityToken accessToken)
                        {
                            var identity = context.Principal?.Identity as ClaimsIdentity;
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

    public static void AddAmazonServices(this IServiceCollection services)
    {
        services.AddScoped<ITranslationService, TranslationService>();
        services.AddScoped<IS3Service, S3Service>();
        services.AddScoped<IComprehendService, ComprehendService>();
    }

    public static void AddCorsConfig(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddCors(options =>
        {
            options.AddDefaultPolicy(builder =>
            {
                builder.WithOrigins(configuration["Cors"])
                    .AllowCredentials()
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });
        });
    }
}