using Giantnodes.Infrastructure.Faults;
using Giantnodes.Service.Orchestrator.Application.Contracts.Directories.Queries;
using Giantnodes.Service.Orchestrator.Domain.Aggregates.Entries.Directories.Repositories;
using Giantnodes.Service.Orchestrator.Domain.Aggregates.Entries.Files;
using MassTransit;

namespace Giantnodes.Service.Orchestrator.Application.Components.Directories.Queries;

public class GetFileResolutionDistributionConsumer : IConsumer<GetFileResolutionDistribution.Query>
{
    private readonly IFileSystemDirectoryRepository _repository;

    public GetFileResolutionDistributionConsumer(IFileSystemDirectoryRepository repository)
    {
        _repository = repository;
    }

    public async Task Consume(ConsumeContext<GetFileResolutionDistribution.Query> context)
    {
        var directory = await _repository.SingleOrDefaultAsync(x => x.Id == context.Message.DirectoryId, context.CancellationToken);
        if (directory == null)
        {
            await context.RejectAsync(FaultKind.NotFound, nameof(context.Message.DirectoryId));
            return;
        }

        var directories = await _repository
            .ToListAsync(x => x.PathInfo.FullName.StartsWith(directory.PathInfo.FullName), context.CancellationToken);

        var distribution = directories
            .SelectMany(x => x.Entries)
            .OfType<FileSystemFile>()
            .GroupBy(x => x.VideoStreams.SingleOrDefault(stream => stream.Index == 0)?.Quality.Resolution)
            .Select(x => new KeyValuePair<int?, int>(x.Key?.Id, x.Count()))
            .ToArray();

        await context.RespondAsync(new GetFileResolutionDistribution.Result { Distribution = distribution });
    }
}