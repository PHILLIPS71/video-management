using Giantnodes.Service.Dashboard.Domain.Aggregates.Libraries.Entities;
using Giantnodes.Service.Dashboard.HttpApi.Libraries.Types.Interfaces;

namespace Giantnodes.Service.Dashboard.HttpApi.Libraries.Types.Objects.Entities;

public class FileSystemDirectoryType : ObjectType<FileSystemDirectory>
{
    protected override void Configure(IObjectTypeDescriptor<FileSystemDirectory> descriptor)
    {
        descriptor.Implements<FileSystemEntryType>();

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

        descriptor
            .Field(p => p.Entries)
            // .UsePaging()
            // .UseProjection()
            .UseFiltering()
            .UseSorting();
    }
}