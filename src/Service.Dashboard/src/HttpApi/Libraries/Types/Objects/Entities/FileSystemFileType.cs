using Giantnodes.Service.Dashboard.Domain.Aggregates.Libraries.Entities;
using Giantnodes.Service.Dashboard.HttpApi.Libraries.Types.Interfaces;

namespace Giantnodes.Service.Dashboard.HttpApi.Libraries.Types.Objects.Entities;

public class FileSystemFileType : ObjectType<FileSystemFile>
{
    protected override void Configure(IObjectTypeDescriptor<FileSystemFile> descriptor)
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
            .Field(p => p.VideoStreams)
            // .UsePaging()
            // .UseProjection()
            .UseFiltering()
            .UseSorting();

        descriptor
            .Field(p => p.AudioStreams)
            // .UsePaging()
            // .UseProjection()
            .UseFiltering()
            .UseSorting();

        descriptor
            .Field(p => p.SubtitleStreams)
            // .UsePaging()
            // .UseProjection()
            .UseFiltering()
            .UseSorting();
    }
}