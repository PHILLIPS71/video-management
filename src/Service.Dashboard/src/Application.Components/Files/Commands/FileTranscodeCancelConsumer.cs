using Giantnodes.Infrastructure.Faults;
using Giantnodes.Infrastructure.Uow.Services;
using Giantnodes.Service.Dashboard.Application.Contracts.Files.Commands;
using Giantnodes.Service.Dashboard.Application.Contracts.Files.Events;
using Giantnodes.Service.Dashboard.Domain.Aggregates.Entries.Files.Repositories;
using Giantnodes.Service.Dashboard.Domain.Shared.Enums;
using MassTransit;

namespace Giantnodes.Service.Dashboard.Application.Components.Files.Commands;

public class FileTranscodeCancelConsumer : IConsumer<FileTranscodeCancel.Command>
{
    private readonly IUnitOfWorkService _uow;
    private readonly IFileSystemFileRepository _fileRepository;

    public FileTranscodeCancelConsumer(IUnitOfWorkService uow, IFileSystemFileRepository fileRepository)
    {
        _uow = uow;
        _fileRepository = fileRepository;
    }

    public async Task Consume(ConsumeContext<FileTranscodeCancel.Command> context)
    {
        using (var uow = _uow.Begin())
        {
            var file = await _fileRepository.SingleOrDefaultAsync(x => x.Id == context.Message.FileId);
            if (file == null)
            {
                await context.RejectAsync(FaultKind.NotFound, nameof(context.Message.FileId));
                return;
            }

            var transcode = file.Transcodes.FirstOrDefault(x => x.Id == context.Message.TranscodeId);
            if (transcode == null)
            {
                await context.RejectAsync(FaultKind.NotFound, nameof(context.Message.TranscodeId));
                return;
            }

            transcode.SetStatus(TranscodeStatus.Cancelled);

            await context.Publish(new FileTranscodeCancelledEvent
            {
                FileId = file.Id,
                TranscodeId = transcode.Id
            }, context.CancellationToken);

            await uow.CommitAsync(context.CancellationToken);
            await context.RespondAsync(new FileTranscodeCancel.Result { TranscodeId = transcode.Id });
        }
    }
}