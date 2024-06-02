using System.Reflection;
using Giantnodes.Infrastructure.MassTransit.Filters;
using Giantnodes.Service.Encoder.Application.Components.Settings;
using Giantnodes.Service.Encoder.Persistence.DbContexts;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;

namespace Giantnodes.Service.Encoder.Console;

public static class ConsoleServiceRegistration
{
    public static void AddConsoleServices(this IServiceCollection services)
    {
        services
            .AddOptions<LimitSettings>()
            .BindConfiguration(LimitSettings.ConfigurationSection)
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddMassTransitServices();
    }

    private static void AddMassTransitServices(this IServiceCollection services)
    {
        services
            .AddMassTransit(options =>
            {
                options
                    .SetKebabCaseEndpointNameFormatter();

                options
                    .AddDelayedMessageScheduler();

                options
                    .SetJobConsumerOptions();

                options
                    .AddJobSagaStateMachines(configure => configure.FinalizeCompleted = true)
                    .EntityFrameworkRepository(configure =>
                    {
                        configure.ConcurrencyMode = ConcurrencyMode.Optimistic;

                        configure.ExistingDbContext<ApplicationDbContext>();
                        configure.UsePostgres();
                    });

                options
                    .AddConsumers(Assembly.Load("Giantnodes.Service.Encoder.Application.Components"));

                options
                    .AddSagaStateMachines(Assembly.Load("Giantnodes.Service.Encoder.Application.Components"));

                options
                    .SetEntityFrameworkSagaRepositoryProvider(configure =>
                    {
                        configure.ConcurrencyMode = ConcurrencyMode.Optimistic;

                        configure.ExistingDbContext<ApplicationDbContext>();
                        configure.UsePostgres();
                    });

                options
                    .AddEntityFrameworkOutbox<ApplicationDbContext>(configure =>
                    {
                        configure.UsePostgres();
                        configure.UseBusOutbox();
                    });

                options
                    .UsingPostgres((context, config) =>
                    {
                        config.UseDelayedMessageScheduler();

                        config.UseConsumeFilter(typeof(FluentValidationFilter<>), context);

                        config.ConfigureEndpoints(context);
                    });
            });
    }
}