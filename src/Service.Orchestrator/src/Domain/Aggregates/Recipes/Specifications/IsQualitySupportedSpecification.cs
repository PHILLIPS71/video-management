using System.Linq.Expressions;
using Giantnodes.Infrastructure.Domain.Specifications;

namespace Giantnodes.Service.Orchestrator.Domain.Aggregates.Recipes.Specifications;

public class IsQualitySupportedSpecification : Specification<Recipe>
{
    public override Expression<Func<Recipe, bool>> ToExpression()
    {
        return x => x.Quality == null ||
                    (x.Quality.Value >= x.Codec.Quality.Min && x.Quality.Value <= x.Codec.Quality.Max);
    }
}