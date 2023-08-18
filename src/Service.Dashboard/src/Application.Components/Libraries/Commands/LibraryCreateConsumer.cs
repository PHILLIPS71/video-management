using System.IO.Abstractions;
using EntityFramework.Exceptions.Common;
using Giantnodes.Infrastructure.Faults;
using Giantnodes.Service.Dashboard.Application.Contracts.Libraries.Commands;
using Giantnodes.Service.Dashboard.Domain.Aggregates.Libraries.Entities;
using Giantnodes.Service.Dashboard.Domain.Aggregates.Libraries.Services;
using Giantnodes.Service.Dashboard.Persistence.DbContexts;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace Giantnodes.Service.Dashboard.Application.Components.Libraries.Commands;

public class LibraryCreateConsumer : IConsumer<LibraryCreate.Command>
{
    private readonly ApplicationDbContext _database;
    private readonly IFileSystemService _fileSystemService;
    private readonly IFileSystem _fileSystem;

    public LibraryCreateConsumer(ApplicationDbContext database, IFileSystemService fileSystemService, IFileSystem fileSystem)
    {
        _database = database;
        _fileSystemService = fileSystemService;
        _fileSystem = fileSystem;
    }

    public async Task Consume(ConsumeContext<LibraryCreate.Command> context)
    {
        var directory = _fileSystem.DirectoryInfo.New(context.Message.FullPath);

        var library = new Library(directory, context.Message.Name, context.Message.Slug);
        library.Scan(_fileSystemService);

        _database.Libraries.Add(library);

        try
        {
            await _database.SaveChangesAsync(context.CancellationToken);
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