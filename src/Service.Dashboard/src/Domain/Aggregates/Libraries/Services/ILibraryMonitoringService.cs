using Giantnodes.Infrastructure.DependencyInjection;
using Giantnodes.Infrastructure.Domain.Services;

namespace Giantnodes.Service.Dashboard.Domain.Aggregates.Libraries.Services;

public interface ILibraryMonitoringService : IDomainService, ISingletonDependency
{
    public Task TryMonitorAsync(Library library);

    public void TryUnMonitor(Library library);
}