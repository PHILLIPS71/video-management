using System.IO.Abstractions;
using Giantnodes.Service.Dashboard.Application.Contracts.Libraries.Commands;
using Giantnodes.Service.Dashboard.Domain.Entities.Libraries;
using Giantnodes.Service.Dashboard.Persistence.DbContexts;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace Giantnodes.Service.Dashboard.Application.Components.Libraries.Commands;

public class CreateLibraryConsumer : IConsumer<CreateLibrary.Command>
{
    private readonly ApplicationDbContext _database;
    private readonly IFileSystem _system;

    public CreateLibraryConsumer(ApplicationDbContext database, IFileSystem system)
    {
        _database = database;
        _system = system;
    }

    public async Task Consume(ConsumeContext<CreateLibrary.Command> context)
    {
        var duplicate = await _database
            .Libraries
            .AnyAsync(
                x => x.Name == context.Message.Name ||
                     x.Slug == context.Message.Slug ||
                     x.FullPath == context.Message.FullPath,
                context.CancellationToken);

        if (duplicate)
        {
            await context.RejectAsync<CreateLibrary.Rejected, CreateLibrary.Rejection>(CreateLibrary.Rejection.Duplicate);
            return;
        }

        var directory = _system.DirectoryInfo.New(context.Message.FullPath);
        if (directory.Exists == false)
        {
            await context.RejectAsync<CreateLibrary.Rejected, CreateLibrary.Rejection>(CreateLibrary.Rejection.NotFound);
            return;
        }

        var library = new Library
        {
            Name = context.Message.Name,
            Slug = context.Message.Slug,
            FullPath = directory.FullName
        };

        _database.Libraries.Add(library);
        await _database.SaveChangesAsync(context.CancellationToken);
        await context.RespondAsync<CreateLibrary.Result>(new { library.Id });
    }
}