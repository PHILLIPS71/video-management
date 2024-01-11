﻿using Giantnodes.Infrastructure.Faults;
using Giantnodes.Infrastructure.Uow.Services;
using Giantnodes.Service.Dashboard.Application.Contracts.Files.Commands;
using Giantnodes.Service.Dashboard.Application.Contracts.Files.Events;
using Giantnodes.Service.Dashboard.Domain.Aggregates.Entries.Files.Repositories;
using MassTransit;

namespace Giantnodes.Service.Dashboard.Application.Components.Files.Commands;

public class FileEncodeSubmitConsumer : IConsumer<FileEncodeSubmit.Command>
{
    private readonly IUnitOfWorkService _uow;
    private readonly IFileSystemFileRepository _fileRepository;

    public FileEncodeSubmitConsumer(IUnitOfWorkService uow, IFileSystemFileRepository fileRepository)
    {
        _uow = uow;
        _fileRepository = fileRepository;
    }

    public async Task Consume(ConsumeContext<FileEncodeSubmit.Command> context)
    {
        var file = await _fileRepository.SingleOrDefaultAsync(x => x.Id == context.Message.FileId);
        if (file == null)
        {
            await context.RejectAsync(FaultKind.NotFound, nameof(context.Message.FileId));
            return;
        }

        using (var uow = await _uow.BeginAsync(context.CancellationToken))
        {
            var encode = file.Encode();

            await context.Publish(new FileEncodeCreatedEvent
            {
                FileId = file.Id,
                EncodeId = encode.Id,
                FullPath = file.PathInfo.FullName
            }, context.CancellationToken);

            await uow.CommitAsync(context.CancellationToken);
            await context.RespondAsync(new FileEncodeSubmit.Result { EncodeId = encode.Id });
        }
    }
}