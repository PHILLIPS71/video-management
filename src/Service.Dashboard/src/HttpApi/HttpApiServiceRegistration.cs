﻿using System.Reflection;
using Giantnodes.Infrastructure.GraphQL;
using Giantnodes.Infrastructure.GraphQL.Scalars;
using Giantnodes.Infrastructure.Masstransit.Validation;
using Giantnodes.Service.Dashboard.HttpApi.Cors;
using Giantnodes.Service.Dashboard.Persistence.DbContexts;
using HotChocolate.Data.Filters;
using HotChocolate.Types.Descriptors;
using HotChocolate.Types.Pagination;
using MassTransit;

namespace Giantnodes.Service.Dashboard.HttpApi;

public static class HttpApiServiceRegistration
{
    public static void AddHttpApiServices(this IServiceCollection services)
    {
        services.AddCors().ConfigureOptions<CorsConfigureOptions>();
        services.AddGraphQLServices();
        services.AddMassTransitServices();
    }

    private static void AddGraphQLServices(this IServiceCollection services)
    {
        services
            .AddGraphQLServer()
            .ModifyOptions(opt => opt.DefaultFieldBindingFlags = FieldBindingFlags.Default)
            .SetPagingOptions(new PagingOptions { IncludeTotalCount = true })
            .RegisterDbContext<ApplicationDbContext>(DbContextKind.Pooled)
            .AddType<CharType>()
            .AddConvention<IFilterConvention, CharFilterConvention>()
            .AddConvention<INamingConventions, SnakeCaseNamingConvention>()
            .AddGlobalObjectIdentification()
            .AddMutationConventions()
            .AddHttpApiTypes()
            .AddQueryType()
            .AddMutationType()
            .AddProjections()
            .AddFiltering()
            .AddSorting();
    }

    private static void AddMassTransitServices(this IServiceCollection services)
    {
        services
            .AddMassTransit(options =>
            {
                options
                    .SetKebabCaseEndpointNameFormatter();

                options
                    .AddConsumers(Assembly.Load("Giantnodes.Service.Dashboard.Application.Components"));

                options
                    .UsingRabbitMq((context, config) =>
                    {
                        config.UseConsumeFilter(typeof(FluentValidationFilter<>), context);

                        config.ConfigureEndpoints(context);
                    });
            });
    }
}