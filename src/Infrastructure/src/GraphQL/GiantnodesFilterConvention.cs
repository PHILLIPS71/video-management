using Giantnodes.Infrastructure.GraphQL.Scalars;
using HotChocolate.Data.Filters;

namespace Giantnodes.Infrastructure.GraphQL;

public sealed class GiantnodesFilterConvention : FilterConvention
{
    protected override void Configure(IFilterConventionDescriptor descriptor)
    {
        base.Configure(descriptor);

        descriptor.BindRuntimeType<char, CharOperationFilterInputType>();
        descriptor.BindRuntimeType<char?, CharOperationFilterInputType>();

        descriptor.BindRuntimeType<uint, UnsignedIntOperationFilterInputType>();
        descriptor.BindRuntimeType<uint?, UnsignedIntOperationFilterInputType>();

        descriptor.AddDefaults();
    }
}