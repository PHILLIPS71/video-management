using Giantnodes.Service.Orchestrator.Domain.Aggregates.Entries.Directories;
using Giantnodes.Service.Orchestrator.HttpApi.Types.Entries.Interfaces;

namespace Giantnodes.Service.Orchestrator.HttpApi.Types.Directories.Objects;

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
            .Field(p => p.ScannedAt);

        descriptor
            .Field(p => p.Entries)
            // .UsePaging()
            // .UseProjection()
            .UseFiltering()
            .UseSorting();
    }
}