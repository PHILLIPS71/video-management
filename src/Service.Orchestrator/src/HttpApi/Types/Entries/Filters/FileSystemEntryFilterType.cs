using Giantnodes.Service.Orchestrator.Domain.Aggregates.Entries;
using HotChocolate.Data.Filters;

namespace Giantnodes.Service.Orchestrator.HttpApi.Types.Entries.Filters;

public class FileSystemEntryFilterType : FilterInputType<FileSystemEntry>
{
    protected override void Configure(IFilterInputTypeDescriptor<FileSystemEntry> descriptor)
    {
        descriptor
            .Field(p => p.Id)
            .Type<IdOperationFilterInputType>();
    }
}