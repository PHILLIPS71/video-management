using Giantnodes.Service.Orchestrator.Domain.Aggregates.Entries;

namespace Giantnodes.Service.Orchestrator.HttpApi.Types.Entries.Interfaces;

public class FileSystemEntryType : InterfaceType<FileSystemEntry>
{
    protected override void Configure(IInterfaceTypeDescriptor<FileSystemEntry> descriptor)
    {
        descriptor.BindFieldsExplicitly();

        descriptor
            .Field(p => p.Id)
            .Type<NonNullType<IdType>>();

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
    }
}