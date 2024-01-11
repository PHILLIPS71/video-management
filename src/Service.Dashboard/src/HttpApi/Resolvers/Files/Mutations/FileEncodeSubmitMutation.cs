using Giantnodes.Infrastructure.Faults.Exceptions;
using Giantnodes.Infrastructure.Faults.Types;
using Giantnodes.Infrastructure.Validation.Exceptions;
using Giantnodes.Service.Dashboard.Application.Contracts.Files.Commands;
using Giantnodes.Service.Dashboard.Domain.Aggregates.Entries.Files.Entities;
using Giantnodes.Service.Dashboard.Persistence.DbContexts;
using MassTransit;

namespace Giantnodes.Service.Dashboard.HttpApi.Resolvers.Files.Mutations;

[ExtendObjectType(OperationTypeNames.Mutation)]
public class FileEncodeSubmitMutation
{
    [Error<DomainException>]
    [Error<ValidationException>]
    [UseSingleOrDefault]
    [UseProjection]
    public async Task<IQueryable<Encode>> FileEncodeSubmit(
        [Service] ApplicationDbContext database,
        [Service] IRequestClient<FileEncodeSubmit.Command> request,
        [ID] Guid id,
        CancellationToken cancellation = default)
    {
        var command = new FileEncodeSubmit.Command
        {
            FileId = id
        };

        Response response = await request.GetResponse<FileEncodeSubmit.Result, DomainFault, ValidationFault>(command, cancellation);
        return response switch
        {
            (_, FileEncodeSubmit.Result result) => database.Encodes.Where(x => x.Id == result.EncodeId),
            (_, DomainFault fault) => throw new DomainException(fault),
            (_, ValidationFault fault) => throw new ValidationException(fault),
            _ => throw new InvalidOperationException()
        };
    }
}