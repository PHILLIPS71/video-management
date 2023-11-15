using Giantnodes.Infrastructure.Uow.Services;
using Giantnodes.Service.Dashboard.Application.Contracts.Files.Commands;
using Giantnodes.Service.Dashboard.Application.Contracts.Files.Events;
using Giantnodes.Service.Dashboard.Domain.Aggregates.Entries.Files.Repositories;
using MassTransit;

namespace Giantnodes.Service.Dashboard.Application.Components.Files.Commands;

public class FileSubmitTranscodeConsumer : IConsumer<FileSubmitTranscode.Command>
{
    private readonly IUnitOfWorkService _uow;
    private readonly IFileSystemFileRepository _fileRepository;

    public FileSubmitTranscodeConsumer(IUnitOfWorkService uow, IFileSystemFileRepository fileRepository)
    {
        _uow = uow;
        _fileRepository = fileRepository;
    }

    public async Task Consume(ConsumeContext<FileSubmitTranscode.Command> context)
    {
        var file = await _fileRepository.SingleOrDefaultAsync(x => x.Id == context.Message.FileId);
        if (file == null)
        {
            await context.RejectAsync(FileSubmitTranscode.Fault.FileNotFound);
            return;
        }

        using (var uow = _uow.Begin())
        {
            var transcode = file.Transcode();

            await context.Publish(new FileTranscodeCreatedEvent
            {
                TranscodeId = transcode.Id,
                FullPath = file.PathInfo.FullName
            }, context.CancellationToken);

            await uow.CommitAsync(context.CancellationToken);
            await context.RespondAsync(new FileSubmitTranscode.Result { TranscodeId = transcode.Id });
        }
    }
}