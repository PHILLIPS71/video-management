using Giantnodes.Infrastructure.Domain.Specifications.Compositions;

namespace Giantnodes.Service.Dashboard.Domain.Aggregates.Recipes.Specifications;

public class IsEncodableSpecification : AndSpecification<Recipe>
{
    public IsEncodableSpecification()
        : base(new IsQualitySupportedSpecification(), new IsTuneSupportedSpecification())
    {
    }
}