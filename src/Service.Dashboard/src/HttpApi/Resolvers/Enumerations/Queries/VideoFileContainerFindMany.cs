using Giantnodes.Infrastructure;
using Giantnodes.Service.Dashboard.Domain.Enumerations;

namespace Giantnodes.Service.Dashboard.HttpApi.Resolvers.Enumerations.Queries;

[QueryType]
internal sealed class VideoFileContainerFindMany
{
    [UsePaging]
    [UseFiltering]
    [UseSorting]
    public IQueryable<VideoFileContainer> VideoFileContainers()
    {
        return Enumeration.GetAll<VideoFileContainer>().AsQueryable();
    }
}