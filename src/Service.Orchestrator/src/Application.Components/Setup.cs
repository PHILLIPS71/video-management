using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Giantnodes.Service.Orchestrator.Application.Components;

public static class Setup
{
    public static IServiceCollection SetupApplicationComponents(this IServiceCollection services)
    {
        services
            .AddValidatorsFromAssembly(Assembly.Load("Giantnodes.Service.Orchestrator.Application.Contracts"));

        return services;
    }
}