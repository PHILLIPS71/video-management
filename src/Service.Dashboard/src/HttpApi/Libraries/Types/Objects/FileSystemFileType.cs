using Giantnodes.Service.Dashboard.Domain.Aggregates.Libraries.Entities;

namespace Giantnodes.Service.Dashboard.HttpApi.Libraries.Types.Objects;

public class FileSystemFileType : ObjectType<FileSystemFile>
{
    protected override void Configure(IObjectTypeDescriptor<FileSystemFile> descriptor)
    {
        descriptor
            .ImplementsNode()
            .IdField(p => p.Id);

        descriptor
            .Field(p => p.Id);

        descriptor
            .Field(p => p.Size);

        descriptor
            .Field(p => p.PathInfo);

        descriptor
            .Field(p => p.Library);

        descriptor
            .Field(p => p.ParentDirectory);
    }
}