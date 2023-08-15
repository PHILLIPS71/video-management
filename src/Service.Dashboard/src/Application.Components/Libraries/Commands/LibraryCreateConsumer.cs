using System.IO.Abstractions;
using EntityFramework.Exceptions.Common;
using Giantnodes.Infrastructure.Faults;
using Giantnodes.Service.Dashboard.Application.Contracts.Libraries.Commands;
using Giantnodes.Service.Dashboard.Domain.Aggregates.Libraries.Entities;
using Giantnodes.Service.Dashboard.Domain.Aggregates.Libraries.Services;
using Giantnodes.Service.Dashboard.Persistence.DbContexts;
using MassTransit;
using Npgsql;

namespace Giantnodes.Service.Dashboard.Application.Components.Libraries.Commands;

public class LibraryCreateConsumer : IConsumer<LibraryCreate.Command>
{
    private readonly ApplicationDbContext _database;
    private readonly IFileSystem _fs;
    private readonly ILibraryService _service;

    public LibraryCreateConsumer(ApplicationDbContext database, IFileSystem fs, ILibraryService service)
    {
        _database = database;
        _fs = fs;
        _service = service;
    }

    public async Task Consume(ConsumeContext<LibraryCreate.Command> context)
    {
        var directory = _fs.DirectoryInfo.New(context.Message.FullPath);

        var library = new Library(directory, context.Message.Name, context.Message.Slug);
        library.Directory.Scan(_service);
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