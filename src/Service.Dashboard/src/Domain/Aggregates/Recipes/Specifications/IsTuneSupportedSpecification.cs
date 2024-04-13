using System.Linq.Expressions;
using Giantnodes.Infrastructure.Domain.Specifications;

namespace Giantnodes.Service.Dashboard.Domain.Aggregates.Recipes.Specifications;

public class IsTuneSupportedSpecification : Specification<Recipe>
{
    public override Expression<Func<Recipe, bool>> ToExpression()
    {
        return x => x.Tune == null || x.Codec.Tunes.Contains(x.Tune);
    }
}