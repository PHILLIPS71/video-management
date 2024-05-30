using Giantnodes.Infrastructure;
using Giantnodes.Service.Dashboard.Domain.Enumerations;

namespace Giantnodes.Service.Dashboard.HttpApi.Resolvers.Enumerations.Queries;

[QueryType]
public class EncodeCodecFindMany
{
    [UsePaging]
    [UseFiltering]
    [UseSorting]
    public IQueryable<EncodeCodec> EncodeCodecs()
    {
        return Enumeration.GetAll<EncodeCodec>().AsQueryable();
    }
}