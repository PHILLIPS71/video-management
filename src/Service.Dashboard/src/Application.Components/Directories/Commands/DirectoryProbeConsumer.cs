using Giantnodes.Infrastructure.Faults;
using Giantnodes.Service.Dashboard.Application.Contracts.Directories.Commands;
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
        var directory = await _repository
            .SingleOrDefaultAsync(x => x.Id == context.Message.DirectoryId, context.CancellationToken);

        if (directory == null)
        {
            await context.RejectAsync(FaultKind.NotFound, nameof(context.Message.DirectoryId));
            return;
        }

        await context.Publish(new ProbeFileSystem.Job { FullPath = directory.PathInfo.FullName });
        await context.RespondAsync(new DirectoryProbe.Result { FullPath = directory.PathInfo.FullName });
    }
}