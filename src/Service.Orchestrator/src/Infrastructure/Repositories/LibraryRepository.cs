using System.Linq.Expressions;
using Giantnodes.Service.Orchestrator.Domain.Aggregates.Libraries;
using Giantnodes.Service.Orchestrator.Domain.Aggregates.Libraries.Repositories;
using Giantnodes.Service.Orchestrator.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace Giantnodes.Service.Orchestrator.Infrastructure.Repositories;

public sealed class LibraryRepository : ILibraryRepository
{
    private readonly ApplicationDbContext _database;

    public LibraryRepository(ApplicationDbContext database)
    {
        _database = database;
    }

    public IQueryable<Library> ToQueryable()
    {
        return _database
            .Libraries
            .Include(x => x.Entries)
            .AsQueryable();
    }

    public Task<bool> ExistsAsync(
        Expression<Func<Library, bool>> predicate,
        CancellationToken cancellation = default)
    {
        return ToQueryable().AnyAsync(predicate, cancellation);
    }

    public Task<Library> SingleAsync(
        Expression<Func<Library, bool>> predicate,
        CancellationToken cancellation = default)
    {
        return ToQueryable().SingleAsync(predicate, cancellation);
    }

    public Task<Library?> SingleOrDefaultAsync(
        Expression<Func<Library, bool>> predicate,
        CancellationToken cancellation = default)
    {
        return ToQueryable().SingleOrDefaultAsync(predicate, cancellation);
    }

    public Task<List<Library>> ToListAsync(
        Expression<Func<Library, bool>> predicate,
        CancellationToken cancellation = default)
    {
        return ToQueryable()
            .Where(predicate)
            .ToListAsync(cancellation);
    }

    public Library Create(Library entity)
    {
        return _database.Libraries.Add(entity).Entity;
    }

    public Library Delete(Library entity)
    {
        return _database.Libraries.Remove(entity).Entity;
    }
}