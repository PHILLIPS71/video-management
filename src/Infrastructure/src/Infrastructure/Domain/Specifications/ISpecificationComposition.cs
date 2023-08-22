using Giantnodes.Infrastructure.Domain.Entities;

namespace Giantnodes.Infrastructure.Domain.Specifications;

public interface ISpecificationComposition<TEntity> : ISpecification<TEntity>
    where TEntity : Entity
{
    ISpecification<TEntity> Left { get; }

    ISpecification<TEntity> Right { get; }
}