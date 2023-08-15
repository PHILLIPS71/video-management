using System.Linq.Expressions;
using Giantnodes.Infrastructure.Domain.Entities;

namespace Giantnodes.Infrastructure.Domain.Specifications;

public interface ISpecification<TEntity> 
    where TEntity : Entity
{
    bool IsSatisfiedBy(TEntity entity);

    Expression<Func<TEntity, bool>> ToExpression();
}