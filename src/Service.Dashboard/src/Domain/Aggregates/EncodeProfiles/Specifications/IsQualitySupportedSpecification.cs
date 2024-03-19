using System.Linq.Expressions;
using Giantnodes.Infrastructure.Domain.Specifications;

namespace Giantnodes.Service.Dashboard.Domain.Aggregates.EncodeProfiles.Specifications;

public class IsQualitySupportedSpecification : Specification<EncodeProfile>
{
    public override Expression<Func<EncodeProfile, bool>> ToExpression()
    {
        return x => x.Quality == null ||
                    (x.Quality.Value >= x.Codec.Quality.Min && x.Quality.Value <= x.Codec.Quality.Max);
    }
}