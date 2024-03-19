using Giantnodes.Infrastructure.Domain.Specifications.Compositions;

namespace Giantnodes.Service.Dashboard.Domain.Aggregates.EncodeProfiles.Specifications;

public class IsEncodableSpecification : AndSpecification<EncodeProfile>
{
    public IsEncodableSpecification()
        : base(new IsQualitySupportedSpecification(), new IsTuneSupportedSpecification())
    {
    }
}