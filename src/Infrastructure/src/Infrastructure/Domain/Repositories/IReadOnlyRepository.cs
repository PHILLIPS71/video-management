using System.Linq.Expressions;
using Giantnodes.Infrastructure.Domain.Objects;

namespace Giantnodes.Infrastructure.Domain.Repositories;

public interface IReadOnlyRepository<TEntity> : IRepository
    where TEntity : IAggregateRoot
{
    Task<TEntity?> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellation = default);
}