using HotChocolate.Data.Filters;
using HotChocolate.Types;

namespace Giantnodes.Infrastructure.GraphQL.Scalars;

public sealed class UnsignedIntOperationFilterInputType : ComparableOperationFilterInputType<UnsignedIntType>
{
    protected override void Configure(IFilterInputTypeDescriptor descriptor)
    {
        descriptor.Name("UnsignedIntOperationFilterInput");
        base.Configure(descriptor);
    }
}