using Giantnodes.Service.Dashboard.Domain.Aggregates.Libraries.Entities;

namespace Giantnodes.Service.Dashboard.HttpApi.Libraries.Types.Objects;

public class FileSystemDirectoryObjectType : ObjectType<FileSystemDirectory>
{
    protected override void Configure(IObjectTypeDescriptor<FileSystemDirectory> descriptor)
    {
        descriptor
            .ImplementsNode()
            .IdField(f => f.Id);

        descriptor
            .Field(p => p.Id);

        descriptor
            .Field(p => p.PathInfo);
    }
}