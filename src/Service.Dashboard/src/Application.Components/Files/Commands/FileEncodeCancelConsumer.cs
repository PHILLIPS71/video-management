using Giantnodes.Infrastructure.Faults;
using Giantnodes.Infrastructure.Uow.Services;
using Giantnodes.Service.Dashboard.Application.Contracts.Files.Commands;
using Giantnodes.Service.Dashboard.Application.Contracts.Files.Events;
using Giantnodes.Service.Dashboard.Domain.Aggregates.Entries.Files.Repositories;
using Giantnodes.Service.Dashboard.Domain.Shared.Enums;
using MassTransit;

namespace Giantnodes.Service.Dashboard.Application.Components.Files.Commands;

public class FileEncodeCancelConsumer : IConsumer<FileEncodeCancel.Command>
{
    private readonly IUnitOfWorkService _uow;
    private readonly IFileSystemFileRepository _fileRepository;

    public FileEncodeCancelConsumer(IUnitOfWorkService uow, IFileSystemFileRepository fileRepository)
    {
        _uow = uow;
        _fileRepository = fileRepository;
    }

    public async Task Consume(ConsumeContext<FileEncodeCancel.Command> context)
    {
        using (var uow = await _uow.BeginAsync(context.CancellationToken))
        {
            var file = await _fileRepository.SingleOrDefaultAsync(x => x.Id == context.Message.FileId);
            if (file == null)
            {
                await context.RejectAsync(FaultKind.NotFound, nameof(context.Message.FileId));
                return;
            }

            var encode = file.Encodes.FirstOrDefault(x => x.Id == context.Message.EncodeId);
            if (encode == null)
            {
                await context.RejectAsync(FaultKind.NotFound, nameof(context.Message.EncodeId));
                return;
            }

            encode.SetStatus(EncodeStatus.Cancelled);

            await context.Publish(new FileEncodeCancelledEvent
            {
                FileId = file.Id,
                EncodeId = encode.Id
            }, context.CancellationToken);

            await uow.CommitAsync(context.CancellationToken);
            await context.RespondAsync(new FileEncodeCancel.Result { EncodeId = encode.Id });
        }
    }
}