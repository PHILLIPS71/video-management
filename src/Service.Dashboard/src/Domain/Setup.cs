using Giantnodes.Service.Dashboard.Domain.Aggregates.Libraries.Services;
using Giantnodes.Service.Dashboard.Domain.Aggregates.Libraries.Services.Impl;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Giantnodes.Service.Dashboard.Domain;

public static class Setup
{
    public static IServiceCollection SetupDomain(this IServiceCollection services)
    {
        // Services
        services.TryAddSingleton<ILibraryMonitoringService, LibraryMonitoringService>();

        return services;
    }
}