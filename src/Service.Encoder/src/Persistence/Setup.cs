using Giantnodes.Infrastructure.EntityFrameworkCore;
using Giantnodes.Service.Encoder.Persistence.DbContexts;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;

namespace Giantnodes.Service.Encoder.Persistence;

public static class Setup
{
    public static IServiceCollection SetupPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddDbContext<ApplicationDbContext>(options =>
            {
                options
                    .UseNpgsql(configuration.GetConnectionString(name: "DatabaseConnection"), optionsBuilder =>
                    {
                        optionsBuilder.MigrationsHistoryTable("__migrations", ApplicationDbContext.Schema);
                        optionsBuilder.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName);
                    })
                    .UseSnakeCaseNamingConvention();
            });

        services
            .AddOptions<SqlTransportOptions>()
            .Configure(options =>
            {
                var builder = new NpgsqlConnectionStringBuilder(configuration.GetConnectionString(name: "DatabaseConnection"));

                options.Host = builder.Host;
                options.Database = builder.Database;
                options.Schema = "transport";
                options.Role = "transport";
                options.Username = builder.Username;
                options.Password = builder.Password;
                options.AdminUsername = builder.Username;
                options.AdminPassword = builder.Password;
            });

        services
            .AddPostgresMigrationHostedService();

        services
            .AddHostedService<MigratorHostedService<ApplicationDbContext>>();

        return services;
    }
}