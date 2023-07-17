using System.Linq.Expressions;
using Giantnodes.Infrastructure.Domain.Entities;

namespace Giantnodes.Infrastructure.Domain.Specifications;

public abstract class Specification<TEntity> : ISpecification<TEntity>
    where TEntity : Entity
{
    public virtual bool IsSatisfiedBy(TEntity entity)
    {
        return ToExpression().Compile()(entity);
    }

    public abstract Expression<Func<TEntity, bool>> ToExpression();
}