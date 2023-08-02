using Giantnodes.Service.Dashboard.Domain.Aggregates.Libraries.Entities;
using Giantnodes.Service.Dashboard.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace Giantnodes.Service.Dashboard.HttpApi.Libraries.Types.Objects;

public class LibraryObjectType : ObjectType<Library>
{
    protected override void Configure(IObjectTypeDescriptor<Library> descriptor)
    {
        descriptor
            .ImplementsNode()
            .IdField(p => p.Id)
            .ResolveNode((context, id) =>
                context.Service<ApplicationDbContext>().Libraries.SingleOrDefaultAsync(x => x.Id == id));

        descriptor
            .Field(p => p.Name);

        descriptor
            .Field(p => p.Slug);

        descriptor
            .Field(p => p.DriveStatus);

        descriptor
            .Field(p => p.PathInfo);

        descriptor
            .Field(p => p.Entries)
            .UseProjection()
            .UseFiltering()
            .UseSorting();
    }
}