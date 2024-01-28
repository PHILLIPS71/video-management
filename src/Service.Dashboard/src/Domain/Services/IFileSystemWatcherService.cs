using Giantnodes.Infrastructure.DependencyInjection;
using Giantnodes.Infrastructure.Domain.Services;

namespace Giantnodes.Service.Dashboard.Domain.Services;

public interface IFileSystemWatcherService : IApplicationService, ISingletonDependency
{
    public Task<bool> TryWatchAsync<TEvent>(string path, Func<FileSystemEventArgs, TEvent> raise) where TEvent : class;

    public bool TryUnwatch(string path);
}