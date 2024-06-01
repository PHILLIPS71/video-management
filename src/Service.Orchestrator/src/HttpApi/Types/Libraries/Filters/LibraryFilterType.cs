using Giantnodes.Service.Orchestrator.Domain.Aggregates.Libraries;
using HotChocolate.Data.Filters;

namespace Giantnodes.Service.Orchestrator.HttpApi.Types.Libraries.Filters;

public class LibraryFilterType : FilterInputType<Library>
{
    protected override void Configure(IFilterInputTypeDescriptor<Library> descriptor)
    {
        descriptor
            .Field(p => p.Id)
            .Type<IdOperationFilterInputType>();
    }
}