using System.Linq.Expressions;
using Giantnodes.Infrastructure.Domain.Objects;

namespace Giantnodes.Infrastructure.Domain.Repositories;

public interface IRepository<TEntity>
    where TEntity : IAggregateRoot
{
    Task<TEntity?> SingleOrDefaultAsync(
        Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellation = default);

    Task<List<TEntity>> ToListAsync(
        Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellation = default);

    TEntity Create(TEntity entity);
}