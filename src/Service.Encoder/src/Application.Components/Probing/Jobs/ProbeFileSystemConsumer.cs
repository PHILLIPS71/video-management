using System.IO.Abstractions;
using Giantnodes.Service.Encoder.Application.Contracts.Probing.Events;
using Giantnodes.Service.Encoder.Application.Contracts.Probing.Jobs;
using MassTransit;
using Xabe.FFmpeg;

namespace Giantnodes.Service.Encoder.Application.Components.Probing.Jobs;

public class ProbeFileSystemConsumer : IJobConsumer<ProbeFileSystem>
{
    private readonly IFileSystem _system;

    public ProbeFileSystemConsumer(IFileSystem system)
    {
        _system = system;
    }

    public async Task Run(JobContext<ProbeFileSystem> context)
    {
        var exists = _system.Path.Exists(context.Job.FullPath);
        if (exists == false)
            throw new FileNotFoundException($"Could not find a file or directory at '{context.Job.FullPath}'.");

        var files = new List<IFileInfo>();
        switch (_system.File.GetAttributes(context.Job.FullPath))
        {
            case FileAttributes.Directory:
                files = _system.DirectoryInfo.New(context.Job.FullPath)
                    .GetFiles("*", SearchOption.AllDirectories)
                    .ToList();
                break;

            default:
                files.Add(_system.FileInfo.New(context.Job.FullPath));
                break;
        }

        var tasks = files
            .Select(file => Task.Run(async () =>
            {
                var media = await FFmpeg.GetMediaInfo(file.FullName, context.CancellationToken);
                await context.Publish(new ProbedFileEvent
                {
                    JobId = context.JobId,
                    FullPath = file.FullName,
                    Name = Path.GetFileName(file.FullName),
                    Size = file.Length,
                    Timestamp = DateTime.UtcNow,
                    VideoStreams = media
                        .VideoStreams
                        .Select(stream => new Mapper().Map(stream))
                        .ToArray(),
                    AudioStreams = media
                        .AudioStreams
                        .Select(stream => new Mapper().Map(stream))
                        .ToArray(),
                    SubtitleStreams = media
                        .SubtitleStreams
                        .Select(stream => new Mapper().Map(stream))
                        .ToArray()
                }, context.CancellationToken);
            }))
            .ToList();

        await Task.WhenAll(tasks);
    }
}