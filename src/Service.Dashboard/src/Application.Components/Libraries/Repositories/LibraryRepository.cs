using System.Linq.Expressions;
using Giantnodes.Service.Dashboard.Domain.Aggregates.Libraries.Entities;
using Giantnodes.Service.Dashboard.Domain.Aggregates.Libraries.Repositories;
using Giantnodes.Service.Dashboard.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace Giantnodes.Service.Dashboard.Application.Components.Libraries.Repositories;

public sealed class LibraryRepository : ILibraryRepository
{
    private readonly ApplicationDbContext _database;

    public LibraryRepository(ApplicationDbContext database)
    {
        _database = database;
    }

    public Task<Library?> SingleOrDefaultAsync(Expression<Func<Library, bool>> predicate, CancellationToken cancellation = default)
    {
        return _database
            .Libraries
            .Include(x => x.Entries)
            .SingleOrDefaultAsync(predicate, cancellation);
    }
}