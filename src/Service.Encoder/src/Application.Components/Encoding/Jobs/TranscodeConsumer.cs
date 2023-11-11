using System.IO.Abstractions;
using Giantnodes.Service.Encoder.Application.Contracts.Encoding.Events;
using Giantnodes.Service.Encoder.Application.Contracts.Encoding.Jobs;
using MassTransit;
using Serilog;
using Xabe.FFmpeg;
using Xabe.FFmpeg.Events;

namespace Giantnodes.Service.Encoder.Application.Components.Encoding.Jobs;

public class TranscodeConsumer : IJobConsumer<Transcode.Job>
{
    private readonly IFileSystem _fs;

    public TranscodeConsumer(IFileSystem fs)
    {
        _fs = fs;
    }

    public async Task Run(JobContext<Transcode.Job> context)
    {
        var file = _fs.FileInfo.New(context.Job.FullPath);
        if (!file.Exists)
        {
            await context.RejectAsync(Transcode.Fault.PathNotFound, nameof(context.Job.FullPath));
            return;
        }

        var name = Path.ChangeExtension(context.JobId.ToString(), file.Extension);
        if (!string.IsNullOrWhiteSpace(context.Job.Container))
            name = Path.ChangeExtension(name, context.Job.Container);

        var media = await FFmpeg.GetMediaInfo(file.FullName, context.CancellationToken);

        var video = media
            .VideoStreams.First().SetCodec(VideoCodec.h264);

        var output = Path.Join(_fs.Path.GetTempPath(), name);
        var conversion = FFmpeg.Conversions
            .New()
            .AddStream(video)
            .SetOutput(output)
            .SetOverwriteOutput(true)
            .UseMultiThread(true);

        ConversionProgressEventArgs? progress = null;
        conversion.OnProgress += async (_, args) =>
        {
            if (progress?.Percent == args.Percent)
                return;

            await context.Publish(new TranscodeProgressedEvent
            {
                JobId = context.JobId,
                FullPath = output,
                Duration = args.Duration,
                TotalLength = args.TotalLength,
                Percent = args.Percent / 100.0f
            }, context.CancellationToken);

            progress = args;
            Log.Information("transcode progress on file {0} with job id {1} is {2:P}.", output, context.JobId, args.Percent / 100.0f);
        };

        await conversion.Start(context.CancellationToken);
    }
}