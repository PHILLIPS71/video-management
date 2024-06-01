using Giantnodes.Service.Orchestrator.Domain.Values;

namespace Giantnodes.Service.Orchestrator.HttpApi.Types;

public class PathInfoType : ObjectType<PathInfo>
{
    protected override void Configure(IObjectTypeDescriptor<PathInfo> descriptor)
    {
        descriptor
            .Field(p => p.Name);

        descriptor
            .Field(p => p.FullName);

        descriptor
            .Field(p => p.Extension);

        descriptor
            .Field(p => p.DirectoryPath);

        descriptor
            .Field(p => p.DirectorySeparatorChar);
    }
}