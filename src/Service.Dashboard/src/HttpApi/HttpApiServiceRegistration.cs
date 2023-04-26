using System.IO.Abstractions;
using Giantnodes.Infrastructure.GraphQL;
using HotChocolate.Types.Descriptors;
using MassTransit;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Giantnodes.Service.Dashboard.HttpApi;

public static class HttpApiServiceRegistration
{
    public static void AddHttpApiServices(this IServiceCollection services)
    {
        services.TryAddSingleton<IFileSystem, FileSystem>();
        
        services.AddGraphQLServices();
        services.AddMassTransitServices();
    }

    private static void AddGraphQLServices(this IServiceCollection services)
    {
        services
            .AddGraphQLServer()
            .AddConvention<INamingConventions, SnakeCaseNamingConvention>()
            .AddQueryType()
            .AddMutationType()
            .AddHttpApiTypes();
    }

    private static void AddMassTransitServices(this IServiceCollection services)
    {
        services
            .AddMassTransit(options =>
            {
                options
                    .UsingRabbitMq((context, config) =>
                    {
                        config.ConfigureEndpoints(context);
                    });
            });
    }
}