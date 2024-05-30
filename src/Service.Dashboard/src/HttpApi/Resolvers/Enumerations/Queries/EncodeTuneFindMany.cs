using Giantnodes.Infrastructure;
using Giantnodes.Service.Dashboard.Domain.Enumerations;

namespace Giantnodes.Service.Dashboard.HttpApi.Resolvers.Enumerations.Queries;

[QueryType]
public class EncodeTuneFindMany
{
    [UsePaging]
    [UseFiltering]
    [UseSorting]
    public IQueryable<EncodeTune> EncodeTunes()
    {
        return Enumeration.GetAll<EncodeTune>().AsQueryable();
    }
}