using System.IO.Abstractions;
using System.Reflection;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Giantnodes.Service.Dashboard.Application;

public static class ApplicationServiceRegistration
{
    public static void AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.TryAddSingleton<IFileSystem, FileSystem>();

        services.AddMassTransitServices(configuration);
    }

    private static void AddMassTransitServices(this IServiceCollection services, IConfiguration configuration)
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
                        config.ConfigureEndpoints(context);
                    });
            });
    }
}