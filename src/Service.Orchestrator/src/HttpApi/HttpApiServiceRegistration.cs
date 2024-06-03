using System.Reflection;
using Giantnodes.Infrastructure.GraphQL;
using Giantnodes.Infrastructure.GraphQL.Scalars;
using Giantnodes.Infrastructure.MassTransit.Filters;
using Giantnodes.Service.Orchestrator.HttpApi.Cors;
using Giantnodes.Service.Orchestrator.Persistence.DbContexts;
using HotChocolate.Data.Filters;
using HotChocolate.Types.Descriptors;
using HotChocolate.Types.Pagination;
using MassTransit;

namespace Giantnodes.Service.Orchestrator.HttpApi;

public static class HttpApiServiceRegistration
{
    public static void AddHttpApiServices(this IServiceCollection services)
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
                    .AddConsumers(Assembly.Load("Giantnodes.Service.Orchestrator.Application.Components"));

                options
                    .AddSagaStateMachines(Assembly.Load("Giantnodes.Service.Orchestrator.Application.Components"));

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
            .ModifyOptions(opt => opt.DefaultFieldBindingFlags = FieldBindingFlags.Default)
            .SetPagingOptions(new PagingOptions { IncludeTotalCount = true })
            .RegisterDbContext<ApplicationDbContext>(DbContextKind.Pooled)
            .AddInMemorySubscriptions()
            .AddType<CharType>()
            .AddType<UnsignedIntType>()
            .AddConvention<IFilterConvention, GiantnodesFilterConvention>()
            .AddConvention<INamingConventions, SnakeCaseNamingConvention>()
            .AddGlobalObjectIdentification()
            .AddMutationConventions()
            .AddQueryFieldToMutationPayloads()
            .AddHttpApiTypes()
            .AddQueryType()
            .AddMutationType()
            .AddSubscriptionType()
            .AddProjections()
            .AddFiltering()
            .AddSorting();
    }
}