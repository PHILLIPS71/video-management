using Giantnodes.Infrastructure.Faults.Exceptions;
using Giantnodes.Infrastructure.Faults.Types;
using Giantnodes.Infrastructure.Validation.Exceptions;
using Giantnodes.Service.Dashboard.Application.Contracts.Files.Commands;
using Giantnodes.Service.Dashboard.Domain.Aggregates.Entries.Files.Entities;
using Giantnodes.Service.Dashboard.Persistence.DbContexts;
using MassTransit;

namespace Giantnodes.Service.Dashboard.HttpApi.Resolvers.Files.Mutations;

[ExtendObjectType(OperationTypeNames.Mutation)]
public class FileEncodeCancelMutation
{
    [Error<DomainException>]
    [Error<ValidationException>]
    [UseSingleOrDefault]
    [UseProjection]
    public async Task<IQueryable<Encode>> FileEncodeCancel(
        [Service] ApplicationDbContext database,
        [Service] IRequestClient<FileEncodeCancel.Command> request,
        [ID] Guid file_id,
        [ID] Guid encode_id,
        CancellationToken cancellation = default)
    {
        var command = new FileEncodeCancel.Command
        {
            FileId = file_id,
            EncodeId = encode_id
        };

        Response response = await request.GetResponse<FileEncodeCancel.Result, DomainFault, ValidationFault>(command, cancellation);
        return response switch
        {
            (_, FileEncodeCancel.Result result) => database.Encodes.Where(x => x.Id == result.EncodeId),
            (_, DomainFault fault) => throw new DomainException(fault),
            (_, ValidationFault fault) => throw new ValidationException(fault),
            _ => throw new InvalidOperationException()
        };
    }
}