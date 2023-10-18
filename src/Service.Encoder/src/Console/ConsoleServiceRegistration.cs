using System.Reflection;
using Giantnodes.Infrastructure.Masstransit.Validation;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;

namespace Giantnodes.Service.Encoder.Console;

public static class ConsoleServiceRegistration
{
    public static void AddConsoleServices(this IServiceCollection services)
    {
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
                    .AddConsumers(Assembly.Load("Giantnodes.Service.Encoder.Application.Components"));

                options
                    .UsingRabbitMq((context, config) =>
                    {
                        config.UseConsumeFilter(typeof(FluentValidationFilter<>), context);

                        config.ConfigureEndpoints(context);
                    });
            });
    }
}