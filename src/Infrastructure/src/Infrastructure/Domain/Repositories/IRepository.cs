using Giantnodes.Infrastructure.Domain.Objects;

namespace Giantnodes.Infrastructure.Domain.Repositories;

// https://docs.abp.io/en/abp/latest/Repositories

/// <summary>
/// Just to mark a class as repository.
/// </summary>
public interface IRepository
{
}

public interface IRepository<TEntity> : IReadOnlyRepository<TEntity>, IWriteOnlyRepository<TEntity>
    where TEntity : IAggregateRoot
{
}