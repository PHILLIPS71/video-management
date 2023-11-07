using System.Linq.Expressions;
using Giantnodes.Service.Dashboard.Domain.Aggregates.Libraries;
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

    /// <summary>
    /// Builds the <see name="Library"/> aggregates consistency boundary.
    /// </summary>
    /// <returns>A <see cref="IQueryable{TEntity}"/> of the objects that make up the consistency boundary.</returns>
    private IQueryable<Library> Build()
    {
        return _database
            .Libraries
            .Include(x => x.Entries)
            .AsQueryable();
    }

    public Task<Library?> SingleOrDefaultAsync(
        Expression<Func<Library, bool>> predicate,
        CancellationToken cancellation = default)
    {
        return Build().SingleOrDefaultAsync(predicate, cancellation);
    }

    public Task<List<Library>> ToListAsync(
        Expression<Func<Library, bool>> predicate,
        CancellationToken cancellation = default)
    {
        return Build()
            .Where(predicate)
            .ToListAsync(cancellation);
    }

    public Library Create(Library entity)
    {
        return _database.Libraries.Add(entity).Entity;
    }
}