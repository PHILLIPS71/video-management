using Giantnodes.Service.Encoder.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Giantnodes.Service.Encoder.Persistence;

public static class Setup
{
    public static IServiceCollection SetupPersistence(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services
            .AddDbContext<ApplicationDbContext>(options =>
            {
                options
                    .UseNpgsql(configuration.GetConnectionString(name: "DatabaseConnection"), optionsBuilder =>
                    {
                        optionsBuilder.MigrationsHistoryTable("__migrations");
                        optionsBuilder.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName);
                    })
                    .UseSnakeCaseNamingConvention();
            });

        return services;
    }
}