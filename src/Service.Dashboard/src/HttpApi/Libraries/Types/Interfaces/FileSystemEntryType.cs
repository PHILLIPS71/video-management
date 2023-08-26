using Giantnodes.Service.Dashboard.Domain.Aggregates.Libraries.Entities;

namespace Giantnodes.Service.Dashboard.HttpApi.Libraries.Types.Interfaces;

public class FileSystemEntryType : InterfaceType<FileSystemEntry>
{
    protected override void Configure(IInterfaceTypeDescriptor<FileSystemEntry> descriptor)
    {
        descriptor.BindFieldsExplicitly();

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