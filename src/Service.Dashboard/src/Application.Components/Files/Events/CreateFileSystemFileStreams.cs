using Giantnodes.Infrastructure.Uow.Services;
using Giantnodes.Service.Dashboard.Domain.Aggregates.Entries.Files.Repositories;
using Giantnodes.Service.Dashboard.Domain.Aggregates.Entries.Files.Values;
using Giantnodes.Service.Encoder.Application.Contracts.Probing.Events;
using MassTransit;
using FileStream = Giantnodes.Service.Dashboard.Domain.Aggregates.Entries.Files.Values.FileStream;

namespace Giantnodes.Service.Dashboard.Application.Components.Files.Events;

public class CreateFileSystemFileStreams : IConsumer<FileProbedEvent>
{
    private readonly IUnitOfWorkService _uow;
    private readonly IFileSystemFileRepository _repository;

    public CreateFileSystemFileStreams(IUnitOfWorkService uow, IFileSystemFileRepository repository)
    {
        _uow = uow;
        _repository = repository;
    }

    public async Task Consume(ConsumeContext<FileProbedEvent> context)
    {
        using (var uow = await _uow.BeginAsync(context.CancellationToken))
        {
            var files = await _repository
                .ToListAsync(x => x.PathInfo.FullName == context.Message.FullPath, context.CancellationToken);

            foreach (var file in files)
            {
                var videos = context.Message.VideoStreams
                    .Select(x => new VideoStream(x.Index, x.Codec, x.Duration, new VideoQuality(x.Width, x.Height, x.AspectRatio), x.Framerate, x.Bitrate, x.PixelFormat))
                    .ToArray();

                var audio = context.Message.AudioStreams
                    .Select(x => new AudioStream(x.Index, x.Codec, x.Title, x.Language, x.Duration, x.Bitrate, x.SampleRate, x.Channels))
                    .ToArray();

                var subtitles = context.Message.AudioStreams
                    .Select(x => new SubtitleStream(x.Index, x.Codec, x.Title, x.Language))
                    .ToArray();

                var streams = new List<FileStream>();
                streams.AddRange(videos);
                streams.AddRange(audio);
                streams.AddRange(subtitles);

                file.SetStreams(streams.ToArray());
            }

            await uow.CommitAsync(context.CancellationToken);
        }
    }
}