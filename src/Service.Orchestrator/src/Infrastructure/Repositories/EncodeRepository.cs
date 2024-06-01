using System.Linq.Expressions;
using Giantnodes.Service.Orchestrator.Domain.Aggregates.Encodes;
using Giantnodes.Service.Orchestrator.Domain.Aggregates.Encodes.Repositories;
using Giantnodes.Service.Orchestrator.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace Giantnodes.Service.Orchestrator.Infrastructure.Repositories;

public class EncodeRepository : IEncodeRepository
{
    private readonly ApplicationDbContext _database;

    public EncodeRepository(ApplicationDbContext database)
    {
        _database = database;
    }

    public IQueryable<Encode> ToQueryable()
    {
        return _database
            .Encodes
            .Include(x => x.File)
            .Include(x => x.Recipe)
            .Include(x => x.Snapshots)
            .AsQueryable();
    }

    public Task<bool> ExistsAsync(
        Expression<Func<Encode, bool>> predicate,
        CancellationToken cancellation = default)
    {
        return ToQueryable().AnyAsync(predicate, cancellation);
    }

    public Task<Encode> SingleAsync(
        Expression<Func<Encode, bool>> predicate,
        CancellationToken cancellation = default)
    {
        return ToQueryable().SingleAsync(predicate, cancellation);
    }

    public Task<Encode?> SingleOrDefaultAsync(
        Expression<Func<Encode, bool>> predicate,
        CancellationToken cancellation = default)
    {
        return ToQueryable().SingleOrDefaultAsync(predicate, cancellation);
    }

    public Task<List<Encode>> ToListAsync(
        Expression<Func<Encode, bool>> predicate,
        CancellationToken cancellation = default)
    {
        return ToQueryable().Where(predicate).ToListAsync(cancellation);
    }

    public Encode Create(Encode entity)
    {
        return _database.Encodes.Add(entity).Entity;
    }

    public Encode Delete(Encode entity)
    {
        return _database.Encodes.Remove(entity).Entity;
    }
}