using System.Linq.Expressions;
using Giantnodes.Infrastructure.Domain.Entities;

namespace Giantnodes.Infrastructure.Domain.Specifications.Compositions;

public class AnySpecification<TEntity> : Specification<TEntity>
    where TEntity : Entity
{
    public override Expression<Func<TEntity, bool>> ToExpression()
    {
        return entity => true;
    }
}