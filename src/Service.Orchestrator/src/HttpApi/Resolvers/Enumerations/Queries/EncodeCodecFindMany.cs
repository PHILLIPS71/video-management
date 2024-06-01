using Giantnodes.Infrastructure;
using Giantnodes.Service.Orchestrator.Domain.Enumerations;

namespace Giantnodes.Service.Orchestrator.HttpApi.Resolvers.Enumerations.Queries;

[QueryType]
internal sealed class EncodeCodecFindMany
{
    [UsePaging]
    [UseFiltering]
    [UseSorting]
    public IQueryable<EncodeCodec> EncodeCodecs()
    {
        return Enumeration.GetAll<EncodeCodec>().AsQueryable();
    }
}