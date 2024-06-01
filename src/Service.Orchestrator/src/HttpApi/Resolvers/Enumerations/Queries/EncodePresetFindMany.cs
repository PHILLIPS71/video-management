using Giantnodes.Infrastructure;
using Giantnodes.Service.Orchestrator.Domain.Enumerations;

namespace Giantnodes.Service.Orchestrator.HttpApi.Resolvers.Enumerations.Queries;

[ExtendObjectType(OperationTypeNames.Query)]
public class EncodePresetFindMany
{
    [UsePaging]
    [UseFiltering]
    [UseSorting]
    public IQueryable<EncodePreset> EncodePresets()
    {
        return Enumeration.GetAll<EncodePreset>().AsQueryable();
    }
}