using Giantnodes.Service.Orchestrator.Domain.Aggregates.Libraries;
using Giantnodes.Service.Orchestrator.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace Giantnodes.Service.Orchestrator.HttpApi.Types.Libraries.Objects;

public class LibraryType : ObjectType<Library>
{
    protected override void Configure(IObjectTypeDescriptor<Library> descriptor)
    {
        descriptor
            .ImplementsNode()
            .IdField(p => p.Id)
            .ResolveNode((context, id) =>
                context.Service<ApplicationDbContext>().Libraries.SingleOrDefaultAsync(x => x.Id == id));

        descriptor
            .Field(p => p.Id);

        descriptor
            .Field(p => p.Name);

        descriptor
            .Field(p => p.Slug);

        descriptor
            .Field(p => p.PathInfo);

        descriptor
            .Field(p => p.Status);

        descriptor
            .Field(p => p.IsWatched);

        descriptor
            .Field(p => p.Entries)
            .UseProjection()
            .UseFiltering()
            .UseSorting();
    }
}