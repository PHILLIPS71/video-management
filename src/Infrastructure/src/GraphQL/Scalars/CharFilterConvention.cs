using HotChocolate.Data.Filters;

namespace Giantnodes.Infrastructure.GraphQL.Scalars;

public class CharFilterConvention : FilterConvention
{
    protected override void Configure(IFilterConventionDescriptor descriptor)
    {
        base.Configure(descriptor);

        descriptor.BindRuntimeType<char, CharOperationFilterInputType>();
        descriptor.BindRuntimeType<char?, CharOperationFilterInputType>();
        descriptor.AddDefaults();
    }
}