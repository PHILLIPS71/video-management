using System.Diagnostics;
using System.IO.Abstractions;
using Giantnodes.Service.Encoder.Application.Contracts.Probing.Events;
using Giantnodes.Service.Encoder.Application.Contracts.Probing.Jobs;
using MassTransit;
using MassTransit.Events;
using Serilog;
using Xabe.FFmpeg;

namespace Giantnodes.Service.Encoder.Application.Components.Probing.Jobs;

public class ProbeFileSystemConsumer : IJobConsumer<ProbeFileSystem.Command>
{
    private readonly IFileSystem _fs;

    public ProbeFileSystemConsumer(IFileSystem fs)
    {
        _fs = fs;
    }

    public async Task Run(JobContext<ProbeFileSystem.Command> context)
    {
        var exists = _fs.Path.Exists(context.Job.FullPath);
        if (!exists)
        {
            await context.RejectAsync(ProbeFileSystem.Fault.PathNotFound, nameof(context.Job.FullPath));
            return;
        }

        var files = new List<IFileInfo>();
        switch (_fs.File.GetAttributes(context.Job.FullPath))
        {
            case FileAttributes.Directory:
                files = _fs.DirectoryInfo.New(context.Job.FullPath)
                    .GetFiles("*", SearchOption.AllDirectories)
                    .ToList();
                break;

            default:
                files.Add(_fs.FileInfo.New(context.Job.FullPath));
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
                        JobId = Guid.Empty,
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

                    Log.Information("successfully probed file {0} with job id {1} in {2:000ms}.", media.Path, context.JobId, interval.ElapsedMilliseconds);
                }
                catch (Exception ex)
                {
                    await context.Publish(new FileProbeFaultedEvent
                    {
                        JobId = Guid.Empty,
                        Path = file.FullName,
                        Exception = new FaultExceptionInfo(ex),
                        Timestamp = DateTime.UtcNow,
                    }, context.CancellationToken);

                    Log.Error("failed to probe file {0} with job id {1}.", file.FullName, context.JobId);
                }
            }))
            .ToList();

        await Task.WhenAll(tasks);
    }
}