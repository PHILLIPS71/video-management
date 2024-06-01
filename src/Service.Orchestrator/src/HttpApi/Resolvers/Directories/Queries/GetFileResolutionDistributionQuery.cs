using Giantnodes.Infrastructure;
using Giantnodes.Infrastructure.Faults.Exceptions;
using Giantnodes.Infrastructure.Faults.Types;
using Giantnodes.Infrastructure.Validation.Exceptions;
using Giantnodes.Service.Orchestrator.Application.Contracts.Directories.Queries;
using Giantnodes.Service.Orchestrator.Domain.Enumerations;
using Giantnodes.Service.Orchestrator.HttpApi.Resolvers.Directories.Types;
using MassTransit;

namespace Giantnodes.Service.Orchestrator.HttpApi.Resolvers.Directories.Queries;

[ExtendObjectType(OperationTypeNames.Query)]
public class GetFileResolutionDistributionQuery
{
    [Error<DomainException>]
    [Error<ValidationException>]
    [UseSorting]
    public async Task<IQueryable<FileResolutionDistribution>> FileResolutionDistribution(
        [Service] IRequestClient<GetFileResolutionDistribution.Query> request,
        [ID] Guid directoryId,
        CancellationToken cancellation = default)
    {
        var query = new GetFileResolutionDistribution.Query
        {
            DirectoryId = directoryId
        };

        Response response = await request.GetResponse<GetFileResolutionDistribution.Result, DomainFault, ValidationFault>(query, cancellation);
        return response switch
        {
            (_, GetFileResolutionDistribution.Result result) => result.Distribution
                .Select(x => new FileResolutionDistribution
                {
                    Resolution = Enumeration.TryParse<VideoResolution>(x.Key ?? 0),
                    Count = x.Value
                })
                .AsQueryable(),
            (_, DomainFault fault) => throw new DomainException(fault),
            (_, ValidationFault fault) => throw new ValidationException(fault),
            _ => throw new InvalidOperationException()
        };
    }
}