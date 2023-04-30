using System.IO.Abstractions;
using System.Reflection;
using Giantnodes.Infrastructure.Masstransit.Validation;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Giantnodes.Service.Encoder.Application;

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
                    .AddDelayedMessageScheduler();

                options
                    .AddConsumers(Assembly.Load("Giantnodes.Service.Encoder.Application.Components"));

                options
                    .UsingRabbitMq((context, config) =>
                    {
                        
                        var options = new ServiceInstanceOptions()
                            .SetEndpointNameFormatter(KebabCaseEndpointNameFormatter.Instance)
                            .EnableJobServiceEndpoints();

                        config.ServiceInstance(options, instance =>
                        {
                            instance.ConfigureJobService();
                            instance.ConfigureEndpoints(context);
                        });

                        config.UseDelayedMessageScheduler();
                        config.UseConsumeFilter(typeof(FluentValidationFilter<>), context);
                        config.ConfigureEndpoints(context);
                    });
            });
    }
}