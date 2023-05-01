using System.IO.Abstractions;
using Giantnodes.Service.Dashboard.Application.Contracts.Encoding.Commands;
using Giantnodes.Service.Dashboard.Application.Contracts.Encoding.Events;
using Giantnodes.Service.Dashboard.Domain.Entities.Encoding;
using Giantnodes.Service.Dashboard.Persistence.DbContexts;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace Giantnodes.Service.Dashboard.Application.Components.Encoding.Commands;

public class EncodeSubmitConsumer : IConsumer<EncodeSubmit.Command>
{
    private readonly ApplicationDbContext _database;
    private readonly IFileSystem _system;

    public EncodeSubmitConsumer(ApplicationDbContext database, IFileSystem system)
    {
        _database = database;
        _system = system;
    }

    public async Task Consume(ConsumeContext<EncodeSubmit.Command> context)
    {
        // prevent non-library paths from being encoded
        var isLibraryPath = await _database
            .Libraries
            .AnyAsync(x => context.Message.FullPath.StartsWith(x.FullPath), context.CancellationToken);

        if (isLibraryPath == false)
        {
            await context.RejectAsync<EncodeSubmit.Rejected, EncodeSubmit.Rejection>(EncodeSubmit.Rejection.LibraryNotFound);
            return;   
        }

        var preset = await _database
            .Presets
            .SingleOrDefaultAsync(x => x.Id == context.Message.PresetId, context.CancellationToken);
        
        if (preset == null)
        {
            await context.RejectAsync<EncodeSubmit.Rejected, EncodeSubmit.Rejection>(EncodeSubmit.Rejection.PresetNotFound);
            return;
        }
        
        var info = _system.FileInfo.New(context.Message.FullPath);
        if (info.Exists == false)
        {
            await context.RejectAsync<EncodeSubmit.Rejected, EncodeSubmit.Rejection>(EncodeSubmit.Rejection.FileNotFound);
            return;
        }

        if (info is not IFileInfo)
        {
            await context.RejectAsync<EncodeSubmit.Rejected, EncodeSubmit.Rejection>(EncodeSubmit.Rejection.NotFile);
            return; 
        }

        var encode = new Encode
        {
            PresetId = preset.Id,
            FullPath = info.FullName
        };

        _database.Encodes.Add(encode);
        await _database.SaveChangesAsync(context.CancellationToken);

        await context.Publish<EncodeSubmittedEvent>(new { encode.PresetId, encode.Id, encode.FullPath, InVar.Timestamp });
        await context.RespondAsync<EncodeSubmit.Result>(new { encode.Id });
    }
}