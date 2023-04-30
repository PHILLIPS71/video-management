using Giantnodes.Service.Dashboard.Domain.Entities.Files;
using Giantnodes.Service.Dashboard.Domain.Entities.Files.Streams;
using Giantnodes.Service.Dashboard.Persistence.DbContexts;
using Giantnodes.Service.Encoder.Application.Contracts.Probing.Events;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace Giantnodes.Service.Dashboard.Application.Components.Probing.Events;

public class OnProbedFileConsumer : IConsumer<ProbedFileEvent>
{
    private readonly ApplicationDbContext _database;

    public OnProbedFileConsumer(ApplicationDbContext database)
    {
        _database = database;
    }

    public async Task Consume(ConsumeContext<ProbedFileEvent> context)
    {
        var file = await _database
            .Files
            .Include(x => x.VideoStreams)
            .Include(x => x.AudioStreams)
            .Include(x => x.SubtitleStreams)
            .SingleOrDefaultAsync(x => x.FullPath == context.Message.FullPath, context.CancellationToken);

        if (file == null)
        {
            file = new FileSystemFile
            {
                FullPath = context.Message.FullPath,
                Name = context.Message.Name,
                Size = context.Message.Size
            };

            _database.Files.Add(file);
        }

        file.VideoStreams = context
            .Message
            .VideoStreams
            .Select(stream => new Mapper().Map(stream))
            .ToList();

        file.AudioStreams = context
            .Message
            .AudioStreams
            .Select(stream => new Mapper().Map(stream))
            .ToList();

        file.SubtitleStreams = context
            .Message
            .SubtitleStreams
            .Select(stream => new Mapper().Map(stream))
            .ToList();

        file.Size = context.Message.Size;
        file.ProbedAt = context.Message.Timestamp;
        await _database.SaveChangesAsync(context.CancellationToken);
    }
}