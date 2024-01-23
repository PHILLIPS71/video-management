using Giantnodes.Infrastructure.DependencyInjection;
using Giantnodes.Infrastructure.Domain.Services;

namespace Giantnodes.Service.Dashboard.Domain.Services;

public interface IFileSystemWatcherService : IApplicationService, ISingletonDependency
{
    public Task<bool> TryWatchAsync(string path);

    public bool TryUnwatch(string path);
}