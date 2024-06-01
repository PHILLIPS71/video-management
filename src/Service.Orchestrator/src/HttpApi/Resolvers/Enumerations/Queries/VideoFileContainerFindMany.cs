using Giantnodes.Infrastructure;
using Giantnodes.Service.Orchestrator.Domain.Enumerations;

namespace Giantnodes.Service.Orchestrator.HttpApi.Resolvers.Enumerations.Queries;

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