namespace Giantnodes.Service.Dashboard.Application.Contracts.Directories.Queries;

public sealed class GetFileResolutionDistribution
{
    public sealed record Query
    {
        public required Guid DirectoryId { get; init; }
    }

    public sealed record Result
    {
        public required KeyValuePair<int?, int>[] Distribution { get; init; }
    }
}