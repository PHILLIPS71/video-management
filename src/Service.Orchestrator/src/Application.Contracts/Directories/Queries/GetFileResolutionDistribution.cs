using Giantnodes.Infrastructure.Messages;

namespace Giantnodes.Service.Orchestrator.Application.Contracts.Directories.Queries;

public sealed class GetFileResolutionDistribution
{
    public sealed record Query : Message
    {
        public required Guid DirectoryId { get; init; }
    }

    public sealed record Result
    {
        public required KeyValuePair<int?, int>[] Distribution { get; init; }
    }
}