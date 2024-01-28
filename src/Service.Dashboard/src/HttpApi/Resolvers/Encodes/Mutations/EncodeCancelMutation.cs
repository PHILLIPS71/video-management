using Giantnodes.Infrastructure.Faults.Exceptions;
using Giantnodes.Infrastructure.Faults.Types;
using Giantnodes.Infrastructure.Validation.Exceptions;
using Giantnodes.Service.Dashboard.Application.Contracts.Encodes.Commands;
using Giantnodes.Service.Dashboard.Domain.Aggregates.Encodes;
using Giantnodes.Service.Dashboard.Persistence.DbContexts;
using MassTransit;

namespace Giantnodes.Service.Dashboard.HttpApi.Resolvers.Encodes.Mutations;

[ExtendObjectType(OperationTypeNames.Mutation)]
public class EncodeCancelMutation
{
    [Error<DomainException>]
    [Error<ValidationException>]
    [UseSingleOrDefault]
    [UseProjection]
    public async Task<IQueryable<Encode>> EncodeCancel(
        [Service] ApplicationDbContext database,
        [Service] IRequestClient<EncodeCancel.Command> request,
        [ID] Guid encode_id,
        CancellationToken cancellation = default)
    {
        var command = new EncodeCancel.Command
        {
            EncodeId = encode_id
        };

        Response response = await request.GetResponse<EncodeCancel.Result, DomainFault, ValidationFault>(command, cancellation);
        return response switch
        {
            (_, EncodeCancel.Result result) => database.Encodes.Where(x => x.Id == result.EncodeId),
            (_, DomainFault fault) => throw new DomainException(fault),
            (_, ValidationFault fault) => throw new ValidationException(fault),
            _ => throw new InvalidOperationException()
        };
    }
}