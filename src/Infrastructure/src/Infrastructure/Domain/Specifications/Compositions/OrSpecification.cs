using System.Linq.Expressions;
using Giantnodes.Infrastructure.Domain.Entities;
using Giantnodes.Infrastructure.Expressions;

namespace Giantnodes.Infrastructure.Domain.Specifications.Compositions;

public class OrSpecification<TEntity> : SpecificationComposition<TEntity>
    where TEntity : Entity
{
    public OrSpecification(ISpecification<TEntity> left, ISpecification<TEntity> right)
        : base(left, right)
    {
    }

    public override Expression<Func<TEntity, bool>> ToExpression()
    {
        return Left.ToExpression().Or(Right.ToExpression());
    }
}