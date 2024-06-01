using Giantnodes.Service.Orchestrator.Domain.Aggregates.Libraries.Services;
using Giantnodes.Service.Orchestrator.Domain.Aggregates.Libraries.Services.Impl;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Giantnodes.Service.Orchestrator.Domain;

public static class Setup
{
    public static IServiceCollection SetupDomain(this IServiceCollection services)
    {
        // Services
        services.TryAddSingleton<ILibraryMonitoringService, LibraryMonitoringService>();

        return services;
    }
}