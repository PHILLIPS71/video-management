using Giantnodes.Service.Dashboard.Application.Contracts.Directories;
using Giantnodes.Service.Dashboard.Domain.Aggregates.Entries.Directories.Repositories;
using Giantnodes.Service.Encoder.Application.Contracts.Probing.Jobs;
using MassTransit;

namespace Giantnodes.Service.Dashboard.Application.Components.Directories.Commands;

public class DirectoryProbeConsumer : IConsumer<DirectoryProbe.Command>
{
    private readonly IFileSystemDirectoryRepository _repository;
    
    public DirectoryProbeConsumer(IFileSystemDirectoryRepository repository)
    {
        _repository = repository;
    }

    public async Task Consume(ConsumeContext<DirectoryProbe.Command> context)
    {
        var exists = await _repository
            .ExistsAsync(x => x.PathInfo.FullName == context.Message.FullPath, context.CancellationToken);

        if (!exists)
        {
            await context.RejectAsync(DirectoryProbe.Fault.DirectoryNotFound);
            return;
        }

        await context.Publish(new ProbeFileSystem.Command
        {
            FullPath = context.Message.FullPath
        });

        await context.RespondAsync(new DirectoryProbe.Result { FullPath = context.Message.FullPath });
    }
}