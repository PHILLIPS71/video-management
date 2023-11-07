using EntityFramework.Exceptions.PostgreSQL;
using Giantnodes.Infrastructure.EntityFrameworkCore.Uow;
using Giantnodes.Infrastructure.Uow;
using Giantnodes.Infrastructure.Uow.Services;
using Giantnodes.Infrastructure.Uow.Services.Impl;
using Giantnodes.Service.Dashboard.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Giantnodes.Service.Dashboard.Persistence;

public static class Setup
{
    public static IServiceCollection SetupPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        // services
        //     .AddDbContextPool<ApplicationDbContext>(options =>
        //     {
        //         options
        //             .UseNpgsql(configuration.GetConnectionString(name: "DatabaseConnection"), optionsBuilder =>
        //             {
        //                 optionsBuilder.MigrationsHistoryTable("__migrations");
        //                 optionsBuilder.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName);
        //             })
        //             .UseSnakeCaseNamingConvention()
        //             .UseExceptionProcessor();
        //     });

        services
            .AddDbContext<ApplicationDbContext>(options =>
            {
                options
                    .UseNpgsql(configuration.GetConnectionString(name: "DatabaseConnection"), optionsBuilder =>
                    {
                        optionsBuilder.MigrationsHistoryTable("__migrations");
                        optionsBuilder.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName);
                    })
                    .UseSnakeCaseNamingConvention()
                    .UseExceptionProcessor();
            });

        services.TryAddTransient<IUnitOfWorkService, UnitOfWorkService>();
        services.TryAddTransient<IUnitOfWork, EntityFrameworkUnitOfWork<ApplicationDbContext>>();

        return services;
    }
}