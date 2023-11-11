using Giantnodes.Infrastructure.Faults.Exceptions;
using Giantnodes.Infrastructure.Faults.Types;
using Giantnodes.Infrastructure.Validation.Exceptions;
using Giantnodes.Service.Dashboard.Application.Contracts.Transcoding.Commands;
using Giantnodes.Service.Dashboard.Domain.Aggregates.Entries.Files.Entities;
using Giantnodes.Service.Dashboard.Persistence.DbContexts;
using MassTransit;

namespace Giantnodes.Service.Dashboard.HttpApi.Resolvers.Files.Mutations;

[ExtendObjectType(OperationTypeNames.Mutation)]
public class FileTranscodeMutation
{
    [Error<DomainException>]
    [Error<ValidationException>]
    [UseSingleOrDefault]
    [UseProjection]
    public async Task<IQueryable<Transcode>> FileTranscode(
        [Service] ApplicationDbContext database,
        [Service] IRequestClient<FileTranscode.Command> request,
        Guid id,
        CancellationToken cancellation = default)
    {
        var command = new FileTranscode.Command
        {
            FileId = id
        };

        Response response = await request.GetResponse<FileTranscode.Result, DomainFault, ValidationFault>(command, cancellation);
        return response switch
        {
            (_, FileTranscode.Result result) => database.Transcodes.Where(x => x.Id == result.TranscodeId),
            (_, DomainFault fault) => throw new DomainException(fault),
            (_, ValidationFault fault) => throw new ValidationException(fault),
            _ => throw new InvalidOperationException()
        };
    }
}