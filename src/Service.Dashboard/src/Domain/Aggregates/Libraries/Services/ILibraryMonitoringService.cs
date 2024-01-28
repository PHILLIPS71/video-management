using Giantnodes.Infrastructure.DependencyInjection;
using Giantnodes.Infrastructure.Domain.Services;

namespace Giantnodes.Service.Dashboard.Domain.Aggregates.Libraries.Services;

public interface ILibraryMonitoringService : IDomainService, ISingletonDependency
{
    public Task<bool> TryMonitorAsync(Library library);

    public bool TryUnMonitor(Library library);
}