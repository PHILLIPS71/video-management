﻿using System.IO.Abstractions;
using EntityFramework.Exceptions.Common;
using Giantnodes.Infrastructure.Faults;
using Giantnodes.Service.Dashboard.Application.Contracts.Libraries.Commands;
using Giantnodes.Service.Dashboard.Domain.Aggregates.Libraries;
using Giantnodes.Service.Dashboard.Domain.Aggregates.Libraries.Repositories;
using Giantnodes.Service.Dashboard.Domain.Aggregates.Libraries.Services;
using MassTransit;
using Npgsql;

namespace Giantnodes.Service.Dashboard.Application.Components.Libraries.Commands;

public class LibraryCreateConsumer : IConsumer<LibraryCreate.Command>
{
    private readonly ILibraryRepository _repository;
    private readonly IFileSystem _fileSystem;
    private readonly IFileSystemService _fileSystemService;
    private readonly IFileSystemWatcherService _watcher;

    public LibraryCreateConsumer(
        ILibraryRepository repository,
        IFileSystem fileSystem,
        IFileSystemService fileSystemService,
        IFileSystemWatcherService watcher)
    {
        _repository = repository;
        _fileSystem = fileSystem;
        _fileSystemService = fileSystemService;
        _watcher = watcher;
    }

    public async Task Consume(ConsumeContext<LibraryCreate.Command> context)
    {
        var directory = _fileSystem.DirectoryInfo.New(context.Message.FullPath);
        if (!directory.Exists)
        {
            await context.RejectAsync(LibraryCreate.Fault.DirectoryNotFound, nameof(context.Message.FullPath));
            return;
        }

        var library = new Library(_fileSystemService, directory, context.Message.Name, context.Message.Slug);
        library.Scan(_fileSystemService);

        try
        {
            library.SetWatched(_watcher, context.Message.IsWatched);
        }
        catch (PlatformNotSupportedException)
        {
            await context.RejectAsync(FaultKind.Platform);
            return;
        }

        try
        {
            _repository.Create(library);
            await _repository.SaveChangesAsync(context.CancellationToken);
        }
        catch (UniqueConstraintException ex) when (ex.InnerException is PostgresException pg)
        {
            var param = pg.ConstraintName switch
            {
                "ix_libraries_slug" => nameof(context.Message.Slug),
                _ => null
            };

            await context.RejectAsync(FaultKind.Constraint, param);
            return;
        }

        await context.RespondAsync(new LibraryCreate.Result { Id = library.Id });
    }
}