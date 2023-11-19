using Giantnodes.Infrastructure.Faults.Exceptions;
using Giantnodes.Infrastructure.Faults.Types;
using Giantnodes.Infrastructure.Validation.Exceptions;
using Giantnodes.Service.Dashboard.Application.Contracts.Files.Commands;
using Giantnodes.Service.Dashboard.Domain.Aggregates.Entries.Files.Entities;
using Giantnodes.Service.Dashboard.Persistence.DbContexts;
using MassTransit;

namespace Giantnodes.Service.Dashboard.HttpApi.Resolvers.Files.Mutations;

[ExtendObjectType(OperationTypeNames.Mutation)]
public class FileTranscodeCancelMutation
{
    [Error<DomainException>]
    [Error<ValidationException>]
    [UseSingleOrDefault]
    [UseProjection]
    public async Task<IQueryable<Transcode>> FileTranscodeCancel(
        [Service] ApplicationDbContext database,
        [Service] IRequestClient<FileTranscodeCancel.Command> request,
        [ID] Guid file_id,
        [ID] Guid transcode_id,
        CancellationToken cancellation = default)
    {
        var command = new FileTranscodeCancel.Command
        {
            FileId = file_id,
            TranscodeId = transcode_id
        };

        Response response = await request.GetResponse<FileTranscodeCancel.Result, DomainFault, ValidationFault>(command, cancellation);
        return response switch
        {
            (_, FileTranscodeCancel.Result result) => database.Transcodes.Where(x => x.Id == result.TranscodeId),
            (_, DomainFault fault) => throw new DomainException(fault),
            (_, ValidationFault fault) => throw new ValidationException(fault),
            _ => throw new InvalidOperationException()
        };
    }
}