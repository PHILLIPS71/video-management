using System.Linq.Expressions;
using Giantnodes.Infrastructure.Domain.Specifications;

namespace Giantnodes.Service.Dashboard.Domain.Aggregates.EncodeProfiles.Specifications;

public class IsTuneSupportedSpecification : Specification<EncodeProfile>
{
    public override Expression<Func<EncodeProfile, bool>> ToExpression()
    {
        return x => x.Tune == null || x.Codec.Tunes.Contains(x.Tune);
    }
}