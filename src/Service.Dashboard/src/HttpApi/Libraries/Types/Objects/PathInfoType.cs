using Giantnodes.Service.Dashboard.Domain.Values;

namespace Giantnodes.Service.Dashboard.HttpApi.Libraries.Types.Objects;

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
    }
}