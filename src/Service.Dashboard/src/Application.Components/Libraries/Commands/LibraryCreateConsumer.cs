using System.IO.Abstractions;
using Giantnodes.Service.Dashboard.Application.Contracts.Libraries.Commands;
using Giantnodes.Service.Dashboard.Domain.Aggregates.Libraries.Entities;
using Giantnodes.Service.Dashboard.Domain.Aggregates.Libraries.Services;
using Giantnodes.Service.Dashboard.Persistence.DbContexts;
using MassTransit;

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
        library.Scan(_service);

        _database.Libraries.Add(library);
        await _database.SaveChangesAsync(context.CancellationToken);

        await context.RespondAsync(new LibraryCreate.Result { Id = library.Id });
    }
}