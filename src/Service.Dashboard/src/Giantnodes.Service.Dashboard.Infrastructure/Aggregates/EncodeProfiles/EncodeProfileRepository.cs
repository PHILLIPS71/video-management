using System.Linq.Expressions;
using Giantnodes.Service.Dashboard.Domain.Aggregates.EncodeProfiles;
using Giantnodes.Service.Dashboard.Domain.Aggregates.EncodeProfiles.Repositories;
using Giantnodes.Service.Dashboard.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace Giantnodes.Service.Dashboard.Infrastructure.Aggregates.EncodeProfiles;

public class EncodeProfileRepository : IEncodeProfileRepository
{
    private readonly ApplicationDbContext _database;

    public EncodeProfileRepository(ApplicationDbContext database)
    {
        _database = database;
    }

    /// <summary>
    /// Builds the <see name="EncodeProfile"/> aggregates consistency boundary.
    /// </summary>
    /// <returns>A <see cref="IQueryable{TEntity}"/> of the objects that make up the consistency boundary.</returns>
    private IQueryable<EncodeProfile> Build()
    {
        return _database
            .EncodeProfiles
            .AsQueryable();
    }

    public Task<bool> ExistsAsync(
        Expression<Func<EncodeProfile, bool>> predicate,
        CancellationToken cancellation = default)
    {
        return Build().AnyAsync(predicate, cancellation);
    }

    public Task<EncodeProfile> SingleAsync(
        Expression<Func<EncodeProfile, bool>> predicate,
        CancellationToken cancellation = default)
    {
        return Build().SingleAsync(predicate, cancellation);
    }

    public Task<EncodeProfile?> SingleOrDefaultAsync(
        Expression<Func<EncodeProfile, bool>> predicate,
        CancellationToken cancellation = default)
    {
        return Build().SingleOrDefaultAsync(predicate, cancellation);
    }

    public Task<List<EncodeProfile>> ToListAsync(
        Expression<Func<EncodeProfile, bool>> predicate,
        CancellationToken cancellation = default)
    {
        return Build().Where(predicate).ToListAsync(cancellation);
    }

    public EncodeProfile Create(EncodeProfile entity)
    {
        return _database.EncodeProfiles.Add(entity).Entity;
    }
}