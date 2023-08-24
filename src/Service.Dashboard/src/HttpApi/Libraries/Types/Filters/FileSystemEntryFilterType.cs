using Giantnodes.Service.Dashboard.Domain.Aggregates.Libraries.Entities;
using HotChocolate.Data.Filters;

namespace Giantnodes.Service.Dashboard.HttpApi.Libraries.Types.Filters;

public class FileSystemEntryFilterType : FilterInputType<FileSystemEntry>
{
    protected override void Configure(IFilterInputTypeDescriptor<FileSystemEntry> descriptor)
    {
        descriptor
            .Field(p => p.Id)
            .Type<IdOperationFilterInputType>();
    }
}