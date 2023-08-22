using Giantnodes.Infrastructure.Domain.Entities;
using Giantnodes.Infrastructure.Domain.Specifications.Compositions;

namespace Giantnodes.Infrastructure.Domain.Specifications;

public static class SpecificationExtensions
{
    public static ISpecification<TEntity> And<TEntity>(
        this ISpecification<TEntity> specification,
        ISpecification<TEntity> other)
        where TEntity : Entity
    {
        return new AndSpecification<TEntity>(specification, other);
    }
    
    public static ISpecification<TEntity> Or<TEntity>(
        this ISpecification<TEntity> specification,
        ISpecification<TEntity> other)
        where TEntity : Entity
    {
        return new OrSpecification<TEntity>(specification, other);
    }
}