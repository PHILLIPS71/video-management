using Giantnodes.Service.Orchestrator.Domain.Aggregates.Encodes;
using HotChocolate.Data.Filters;

namespace Giantnodes.Service.Orchestrator.HttpApi.Types.Encodes.Filters;

public class EncodeFilterType : FilterInputType<Encode>
{
    protected override void Configure(IFilterInputTypeDescriptor<Encode> descriptor)
    {
        descriptor
            .Field(p => p.Id)
            .Type<IdOperationFilterInputType>();
    }
}