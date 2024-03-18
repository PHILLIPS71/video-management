using System.Linq.Expressions;
using Giantnodes.Service.Dashboard.Domain.Aggregates.Encodes;
using Giantnodes.Service.Dashboard.Domain.Aggregates.Encodes.Repositories;
using Giantnodes.Service.Dashboard.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace Giantnodes.Service.Dashboard.Infrastructure.Repositories;

public class EncodeRepository : IEncodeRepository
{
    private readonly ApplicationDbContext _database;

    public EncodeRepository(ApplicationDbContext database)
    {
        _database = database;
    }

    /// <summary>
    /// Builds the <see name="Encode"/> aggregates consistency boundary.
    /// </summary>
    /// <returns>A <see cref="IQueryable{TEntity}"/> of the objects that make up the consistency boundary.</returns>
    private IQueryable<Encode> Build()
    {
        return _database
            .Encodes
            .Include(x => x.File)
            .Include(x => x.Profile)
            .Include(x => x.Snapshots)
            .AsQueryable();
    }

    public Task<bool> ExistsAsync(
        Expression<Func<Encode, bool>> predicate,
        CancellationToken cancellation = default)
    {
        return Build().AnyAsync(predicate, cancellation);
    }

    public Task<Encode> SingleAsync(
        Expression<Func<Encode, bool>> predicate,
        CancellationToken cancellation = default)
    {
        return Build().SingleAsync(predicate, cancellation);
    }

    public Task<Encode?> SingleOrDefaultAsync(
        Expression<Func<Encode, bool>> predicate,
        CancellationToken cancellation = default)
    {
        return Build().SingleOrDefaultAsync(predicate, cancellation);
    }

    public Task<List<Encode>> ToListAsync(
        Expression<Func<Encode, bool>> predicate,
        CancellationToken cancellation = default)
    {
        return Build().Where(predicate).ToListAsync(cancellation);
    }

    public Encode Create(Encode entity)
    {
        return _database.Encodes.Add(entity).Entity;
    }
}