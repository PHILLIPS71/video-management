using System.Linq.Expressions;
using Giantnodes.Service.Dashboard.Domain.Aggregates.EncodeProfiles;
using Giantnodes.Service.Dashboard.Domain.Aggregates.EncodeProfiles.Repositories;
using Giantnodes.Service.Dashboard.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace Giantnodes.Service.Dashboard.Infrastructure.Repositories;

public class EncodeProfileRepository : IEncodeProfileRepository
{
    private readonly ApplicationDbContext _database;

    public EncodeProfileRepository(ApplicationDbContext database)
    {
        _database = database;
    }

    public IQueryable<EncodeProfile> ToQueryable()
    {
        return _database
            .EncodeProfiles
            .AsQueryable();
    }

    public Task<bool> ExistsAsync(
        Expression<Func<EncodeProfile, bool>> predicate,
        CancellationToken cancellation = default)
    {
        return ToQueryable().AnyAsync(predicate, cancellation);
    }

    public Task<EncodeProfile> SingleAsync(
        Expression<Func<EncodeProfile, bool>> predicate,
        CancellationToken cancellation = default)
    {
        return ToQueryable().SingleAsync(predicate, cancellation);
    }

    public Task<EncodeProfile?> SingleOrDefaultAsync(
        Expression<Func<EncodeProfile, bool>> predicate,
        CancellationToken cancellation = default)
    {
        return ToQueryable().SingleOrDefaultAsync(predicate, cancellation);
    }

    public Task<List<EncodeProfile>> ToListAsync(
        Expression<Func<EncodeProfile, bool>> predicate,
        CancellationToken cancellation = default)
    {
        return ToQueryable().Where(predicate).ToListAsync(cancellation);
    }

    public EncodeProfile Create(EncodeProfile entity)
    {
        return _database.EncodeProfiles.Add(entity).Entity;
    }

    public EncodeProfile Delete(EncodeProfile entity)
    {
        return _database.EncodeProfiles.Remove(entity).Entity;
    }
}