using Giantnodes.Service.Encoder.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Giantnodes.Service.Encoder.Persistence;

public static class PersistenceServiceRegistration
{
    public static IServiceCollection AddPersistenceServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services
            .AddDbContextPool<ApplicationDbContext>((provider, options) =>
            {
                options
                    .UseNpgsql(configuration.GetConnectionString(name: "DatabaseConnection"), o => o.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName))
                    .UseSnakeCaseNamingConvention();
            });

        services
            .AddPooledDbContextFactory<ApplicationDbContext>((provider, options) =>
            {
                options
                    .UseNpgsql(configuration.GetConnectionString(name: "DatabaseConnection"), o => o.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName))
                    .UseSnakeCaseNamingConvention();
            });

        return services;
    }
}