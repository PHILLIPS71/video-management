using System.IO.Abstractions;
using EntityFramework.Exceptions.Common;
using Giantnodes.Infrastructure.Faults;
using Giantnodes.Infrastructure.Uow.Services;
using Giantnodes.Service.Dashboard.Application.Contracts.Libraries.Commands;
using Giantnodes.Service.Dashboard.Domain.Aggregates.Libraries;
using Giantnodes.Service.Dashboard.Domain.Aggregates.Libraries.Repositories;
using Giantnodes.Service.Dashboard.Domain.Aggregates.Libraries.Services;
using MassTransit;
using Npgsql;

namespace Giantnodes.Service.Dashboard.Application.Components.Libraries.Commands;

public class LibraryCreateConsumer : IConsumer<LibraryCreate.Command>
{
    private readonly IUnitOfWorkService _uow;
    private readonly ILibraryRepository _repository;
    private readonly IFileSystem _fileSystem;
    private readonly IFileSystemService _fileSystemService;
    private readonly IFileSystemWatcherService _watcher;

    public LibraryCreateConsumer(
        IUnitOfWorkService uow,
        ILibraryRepository repository,
        IFileSystem fileSystem,
        IFileSystemService fileSystemService,
        IFileSystemWatcherService watcher)
    {
        _uow = uow;
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
            await context.RejectAsync(FaultKind.NotFound, nameof(context.Message.FullPath));
            return;
        }

        var library = new Library(_fileSystemService, directory, context.Message.Name, context.Message.Slug);
        library.Scan(_fileSystemService);

        try
        {
            if (context.Message.IsWatched)
                library.SetWatched(_watcher, context.Message.IsWatched);
        }
        catch (PlatformNotSupportedException)
        {
            await context.RejectAsync(FaultKind.Platform);
            return;
        }

        using (var uow = await _uow.BeginAsync(context.CancellationToken))
        {
            try
            {
                _repository.Create(library);
                await uow.CommitAsync(context.CancellationToken);
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
        }

        await context.RespondAsync(new LibraryCreate.Result { Id = library.Id });
    }
}