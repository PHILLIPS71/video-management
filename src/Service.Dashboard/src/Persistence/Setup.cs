using EntityFramework.Exceptions.PostgreSQL;
using Giantnodes.Service.Dashboard.Persistence.DbContexts;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;

namespace Giantnodes.Service.Dashboard.Persistence;

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
                        optionsBuilder.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);

                        optionsBuilder.MigrationsHistoryTable("__migrations", ApplicationDbContext.Schema);
                        optionsBuilder.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName);
                    })
                    .UseSnakeCaseNamingConvention()
                    .UseExceptionProcessor();
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

        return services;
    }
}