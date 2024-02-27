using Giantnodes.Infrastructure.Faults.Exceptions;
using Giantnodes.Infrastructure.Faults.Types;
using Giantnodes.Infrastructure.Validation.Exceptions;
using Giantnodes.Service.Dashboard.Application.Contracts.Encodes.Commands;
using Giantnodes.Service.Dashboard.Domain.Aggregates.Encodes;
using Giantnodes.Service.Dashboard.Persistence.DbContexts;
using MassTransit;

namespace Giantnodes.Service.Dashboard.HttpApi.Resolvers.Encodes.Mutations;

[ExtendObjectType(OperationTypeNames.Mutation)]
public class EncodeSubmitMutation
{
    [Error<DomainException>]
    [Error<ValidationException>]
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public async Task<IQueryable<Encode>> EncodeSubmit(
        [Service] ApplicationDbContext database,
        [Service] IRequestClient<EncodeSubmit.Command> request,
        [ID] Guid encode_profile_id,
        [ID] Guid[] entries,
        CancellationToken cancellation = default)
    {
        var command = new EncodeSubmit.Command
        {
            EncodeProfileId = encode_profile_id,
            Entries = entries
        };

        Response response = await request.GetResponse<EncodeSubmit.Result, DomainFault, ValidationFault>(command, cancellation);
        return response switch
        {
            (_, EncodeSubmit.Result result) => database.Encodes.Where(x => result.Encodes.Contains(x.Id)),
            (_, DomainFault fault) => throw new DomainException(fault),
            (_, ValidationFault fault) => throw new ValidationException(fault),
            _ => throw new InvalidOperationException()
        };
    }
}