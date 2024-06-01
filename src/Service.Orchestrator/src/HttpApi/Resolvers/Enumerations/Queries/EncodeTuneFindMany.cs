using Giantnodes.Infrastructure;
using Giantnodes.Service.Orchestrator.Domain.Enumerations;

namespace Giantnodes.Service.Orchestrator.HttpApi.Resolvers.Enumerations.Queries;

[ExtendObjectType(OperationTypeNames.Query)]
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