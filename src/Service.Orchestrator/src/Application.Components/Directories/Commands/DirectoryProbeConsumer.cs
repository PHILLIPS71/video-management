using Giantnodes.Infrastructure.Faults;
using Giantnodes.Service.Orchestrator.Application.Contracts.Directories.Commands;
using Giantnodes.Service.Orchestrator.Domain.Aggregates.Entries.Directories.Repositories;
using Giantnodes.Service.Encoder.Application.Contracts.Probing.Jobs;
using MassTransit;

namespace Giantnodes.Service.Orchestrator.Application.Components.Directories.Commands;

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

        await context.Publish(new ProbeFileSystem.Job { FilePath = directory.PathInfo.FullName });
        await context.RespondAsync(new DirectoryProbe.Result { FilePath = directory.PathInfo.FullName });
    }
}