using System.IO.Abstractions;
using Giantnodes.Service.Dashboard.Application.Contracts.Probing.Commands;
using Giantnodes.Service.Dashboard.Application.Contracts.Probing.Events;
using Giantnodes.Service.Dashboard.Domain.Entities.Probing;
using Giantnodes.Service.Dashboard.Persistence.DbContexts;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace Giantnodes.Service.Dashboard.Application.Components.Probing.Commands;

public class ProbeSubmitConsumer : IConsumer<ProbeSubmit.Command>
{
    private readonly ApplicationDbContext _database;
    private readonly IFileSystem _system;

    public ProbeSubmitConsumer(ApplicationDbContext database, IFileSystem system)
    {
        _database = database;
        _system = system;
    }

    public async Task Consume(ConsumeContext<ProbeSubmit.Command> context)
    {
        // prevent non-library paths from being probed
        var isLibraryPath = await _database
            .Libraries
            .AnyAsync(x => context.Message.FullPath.StartsWith(x.FullPath), context.CancellationToken);

        if (isLibraryPath == false)
        {
            await context.RejectAsync<ProbeSubmit.Rejected, ProbeSubmit.Rejection>(ProbeSubmit.Rejection.LibraryNotFound);
            return;   
        }
        
        var exists = _system.Path.Exists(context.Message.FullPath);
        if (exists == false)
        {
            await context.RejectAsync<ProbeSubmit.Rejected, ProbeSubmit.Rejection>(ProbeSubmit.Rejection.NotFound);
            return;
        }

        var probe = new Probe
        {
            FullPath = context.Message.FullPath
        };

        _database.Probes.Add(probe);
        await _database.SaveChangesAsync(context.CancellationToken);
        
        await context.Publish<ProbeSubmittedEvent>(new { probe.Id, probe.FullPath, InVar.Timestamp });
        await context.RespondAsync<ProbeSubmit.Result>(new { probe.Id });
    }
}