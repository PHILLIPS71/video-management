using System.IO.Abstractions;
using Giantnodes.Service.Dashboard.Persistence;
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
        
        services.AddPersistenceServices(configuration);
    }

    private static void AddMassTransitServices(IServiceCollection services)
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