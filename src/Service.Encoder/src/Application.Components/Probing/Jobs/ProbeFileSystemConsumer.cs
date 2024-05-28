using System.Diagnostics;
using System.IO.Abstractions;
using Giantnodes.Infrastructure.Faults;
using Giantnodes.Service.Encoder.Application.Contracts.Probing.Events;
using Giantnodes.Service.Encoder.Application.Contracts.Probing.Jobs;
using MassTransit;
using MassTransit.Events;
using Microsoft.Extensions.Logging;
using Xabe.FFmpeg;

namespace Giantnodes.Service.Encoder.Application.Components.Probing.Jobs;

public class ProbeFileSystemConsumer : IJobConsumer<ProbeFileSystem.Job>
{
    private readonly IFileSystem _fs;
    private readonly ILogger<ProbeFileSystemConsumer> _logger;

    public ProbeFileSystemConsumer(IFileSystem fs, ILogger<ProbeFileSystemConsumer> logger)
    {
        _fs = fs;
        _logger = logger;
    }

    public async Task Run(JobContext<ProbeFileSystem.Job> context)
    {
        var exists = _fs.Path.Exists(context.Job.FilePath);
        if (!exists)
        {
            await context.RejectAsync(FaultKind.NotFound, nameof(context.Job.FilePath));
            return;
        }

        var files = new List<IFileInfo>();
        switch (_fs.File.GetAttributes(context.Job.FilePath))
        {
            case FileAttributes.Directory:
                files = _fs.DirectoryInfo
                    .New(context.Job.FilePath)
                    .GetFiles("*", SearchOption.AllDirectories)
                    .ToList();
                break;

            default:
                files.Add(_fs.FileInfo.New(context.Job.FilePath));
                break;
        }

        var tasks = files
            .Select(file => Task.Run(async () =>
            {
                var interval = new Stopwatch();

                try
                {
                    interval.Restart();

                    var media = await FFmpeg.GetMediaInfo(file.FullName, context.CancellationToken);
                    await context.Publish(new FileProbedEvent
                    {
                        JobId = context.JobId,
                        CorrelationId = context.Job.CorrelationId,
                        FilePath = file.FullName,
                        Name = Path.GetFileName(file.FullName),
                        Size = file.Length,
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

                    _logger.LogInformation("successfully probed file {FilePath} with job id {JobId} in {Duration:000ms}.", media.Path, context.JobId, interval.ElapsedMilliseconds);
                }
                catch (Exception ex)
                {
                    await context.Publish(new FileProbeFaultedEvent
                    {
                        JobId = context.JobId,
                        CorrelationId = context.Job.CorrelationId,
                        FilePath = file.FullName,
                        Exception = new FaultExceptionInfo(ex),
                    }, context.CancellationToken);

                    _logger.LogError(ex, "failed to probe file {FileName} with job id {JobId}.", file.FullName, context.JobId);
                }
            }))
            .ToList();

        await Task.WhenAll(tasks);
    }
}