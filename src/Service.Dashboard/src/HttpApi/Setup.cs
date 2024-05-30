using System.Reflection;
using Giantnodes.Infrastructure.GraphQL;
using Giantnodes.Infrastructure.GraphQL.Scalars;
using Giantnodes.Infrastructure.MassTransit.Filters;
using Giantnodes.Service.Dashboard.HttpApi.Cors;
using Giantnodes.Service.Dashboard.Persistence.DbContexts;
using HotChocolate.Data.Filters;
using HotChocolate.Types.Descriptors;
using MassTransit;

namespace Giantnodes.Service.Dashboard.HttpApi;

public static class Setup
{
    public static void SetupHttpApiServices(this IServiceCollection services)
    {
        services.AddCors().ConfigureOptions<CorsConfigureOptions>();
        services.AddMassTransitServices();
        services.AddGraphQLServices();
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
                    .AddConsumers(Assembly.Load("Giantnodes.Service.Dashboard.Application.Components"));

                options
                    .AddSagaStateMachines(Assembly.Load("Giantnodes.Service.Dashboard.Application.Components"));

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
                    .AddConfigureEndpointsCallback((context, name, configure) => 
                    {
                        configure.UseEntityFrameworkOutbox<ApplicationDbContext>(context);
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

    private static void AddGraphQLServices(this IServiceCollection services)
    {
        services
            .AddGraphQLServer()
            .ModifyOptions(options => options.DefaultFieldBindingFlags = FieldBindingFlags.Default)
            .AddType<CharType>()
            .AddConvention<IFilterConvention, CharFilterConvention>()
            .AddConvention<INamingConventions, SnakeCaseNamingConvention>()
            .AddGlobalObjectIdentification()
            .AddInMemorySubscriptions()
            .AddMutationConventions()
            .AddHttpApiTypes()
            .AddProjections()
            .AddFiltering()
            .AddSorting()
            .InitializeOnStartup();
    }
}