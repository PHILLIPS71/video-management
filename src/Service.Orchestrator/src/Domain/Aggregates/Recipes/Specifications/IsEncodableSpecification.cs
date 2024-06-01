using Giantnodes.Infrastructure.Domain.Specifications.Compositions;

namespace Giantnodes.Service.Orchestrator.Domain.Aggregates.Recipes.Specifications;

public class IsEncodableSpecification : AndSpecification<Recipe>
{
    public IsEncodableSpecification()
        : base(new IsQualitySupportedSpecification(), new IsTuneSupportedSpecification())
    {
    }
}