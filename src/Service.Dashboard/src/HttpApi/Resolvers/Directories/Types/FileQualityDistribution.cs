using Giantnodes.Service.Dashboard.Domain.Enumerations;

namespace Giantnodes.Service.Dashboard.HttpApi.Resolvers.Directories.Types;

public sealed class FileResolutionDistribution
{
    public required VideoResolution? Resolution { get; init; }

    public required int Count { get; init; }
}

public class FileResolutionDistributionType : ObjectType<FileResolutionDistribution>
{
    protected override void Configure(IObjectTypeDescriptor<FileResolutionDistribution> descriptor)
    {
        descriptor
            .Field(p => p.Resolution);

        descriptor
            .Field(p => p.Count);
    }
}