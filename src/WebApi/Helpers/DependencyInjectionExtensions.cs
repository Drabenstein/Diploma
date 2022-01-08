using Core;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Helpers;

public static class DependencyInjectionExtensions
{
    public static void AddDatabaseServices(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<DiplomaDbContext>(x => x.UseNpgsql(connectionString,
            b => b.MigrationsAssembly("Infrastructure")));
        services.AddScoped<ISqlConnectionFactory>(_ => new SqlConnectionFactory(connectionString));
    }
}