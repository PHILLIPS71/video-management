using Giantnodes.Infrastructure.Domain.Objects;

namespace Giantnodes.Infrastructure.Domain.Repositories;

public interface IWriteOnlyRepository<in TEntity> : IRepository
    where TEntity : IAggregateRoot
{
    Task CreateAsync(TEntity entity, CancellationToken cancellation = default);
    
    Task UpdateAsync(TEntity entity, CancellationToken cancellation = default);
}