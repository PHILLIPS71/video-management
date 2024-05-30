using Giantnodes.Infrastructure;
using Giantnodes.Service.Dashboard.Domain.Enumerations;

namespace Giantnodes.Service.Dashboard.HttpApi.Resolvers.Enumerations.Queries;

[QueryType]
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