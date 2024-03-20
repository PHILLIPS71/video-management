using System.Diagnostics;
using System.IO.Abstractions;
using System.Text.RegularExpressions;
using Giantnodes.Infrastructure.Faults;
using Giantnodes.Service.Encoder.Application.Contracts.Encoding.Events;
using Giantnodes.Service.Encoder.Application.Contracts.Encoding.Jobs;
using MassTransit;
using Serilog;
using Xabe.FFmpeg;
using Xabe.FFmpeg.Events;

namespace Giantnodes.Service.Encoder.Application.Components.Encoding.Jobs;

public class EncodeFileConsumer : IJobConsumer<EncodeFile.Job>
{
    private const double PublishDelaySeconds = 1;
    private const string RegexNumberGroup = "([+-]?[0-9]*[.]?[0-9]*)";

    private readonly IFileSystem _fs;

    public EncodeFileConsumer(IFileSystem fs)
    {
        _fs = fs;
    }

    public async Task Run(JobContext<EncodeFile.Job> context)
    {
        var file = _fs.FileInfo.New(context.Job.InputFilePath);
        if (!file.Exists)
        {
            await context.RejectAsync(FaultKind.NotFound, nameof(context.Job.InputFilePath));
            return;
        }

        var media = await FFmpeg.GetMediaInfo(file.FullName, context.CancellationToken);

        var videos = media
            .VideoStreams
            .Select(stream => stream.SetCodec(context.Job.Codec))
            .ToList();

        var conversion = FFmpeg
            .Conversions
            .New()
            .AddStream(videos)
            .AddParameter($"-preset {context.Job.Preset}")
            .SetOutput(context.Job.OutputFilePath)
            .SetOverwriteOutput(true)
            .UseMultiThread(true);

        if (!string.IsNullOrWhiteSpace(context.Job.Tune))
            conversion.AddParameter($"-tune {context.Job.Tune}");

        if (context.Job.Quality.HasValue)
            conversion.AddParameter($"-crf {context.Job.Quality.Value}");

        if (context.Job.UseHardwareAcceleration)
        {
            var decoder = media.VideoStreams.First().Codec;
            var encoder = context.Job.Codec;

            conversion = conversion
                .UseHardwareAcceleration(nameof(HardwareAccelerator.auto), decoder, encoder);
        }

        var stopwatch = new Stopwatch();
        conversion.OnDataReceived += async (_, args) =>
        {
            if (string.IsNullOrWhiteSpace(args.Data) || args.Data.Contains("N/A"))
                return;

            if (stopwatch.IsRunning && stopwatch.Elapsed <= TimeSpan.FromSeconds(PublishDelaySeconds))
                return;

            try
            {
                if (!TryCreateEvent(context, args.Data, out var @event) || @event == null)
                    return;

                await context.Publish(@event, options => options.Durable = false, context.CancellationToken);
                stopwatch.Restart();
            }
            catch (FormatException ex)
            {
                Log.Error(ex, "Encode data {0} with job id {1} was unable to be parsed.", args.Data, context.JobId);
            }
        };

        ConversionProgressEventArgs? progress = null;
        conversion.OnProgress += async (_, args) =>
        {
            if (progress?.Percent == args.Percent)
                return;

            var @event = new EncodeOperationEncodeProgressedEvent
            {
                JobId = context.JobId,
                CorrelationId = context.Job.CorrelationId,
                Percent = args.Percent / 100.0f
            };

            await context.Publish(@event, ctx => ctx.Durable = false, context.CancellationToken);

            progress = args;
            Log.Information("Encode progress on file {0} with job id {1} is {2:P}.", context.Job.OutputFilePath, context.JobId, args.Percent / 100.0f);
        };

        try
        {
            var @event = new EncodeOperationEncodeBuiltEvent
            {
                JobId = context.JobId,
                CorrelationId = context.Job.CorrelationId,
                FFmpegCommand = conversion.Build()
            };

            await context.Publish(@event, context.CancellationToken);
            await conversion.Start(context.CancellationToken);
        }
        catch (OperationCanceledException)
        {
            var info = _fs.FileInfo.New(context.Job.OutputFilePath);
            if (!info.Exists)
                throw;

            info.Delete();
            Log.Information("Encode with job id {0} was cancelled and file {1} was deleted.", context.JobId, info.FullName);
            throw;
        }
    }

    private static bool TryCreateEvent(JobContext<EncodeFile.Job> context, string stout, out EncodeOperationEncodeHeartbeatEvent? @event)
    {
        @event = null;

        var fps = Regex.Match(stout, $"fps=[ ]*{RegexNumberGroup}");
        if (!fps.Success)
            return false;

        var bitrate = Regex.Match(stout, $"bitrate=[ ]*{RegexNumberGroup}");
        if (!bitrate.Success)
            return false;

        var speed = Regex.Match(stout, $"speed=[ ]*{RegexNumberGroup}x");
        if (!speed.Success)
            return false;

        @event = new EncodeOperationEncodeHeartbeatEvent
        {
            JobId = context.JobId,
            CorrelationId = context.Job.CorrelationId,
            Frames = float.Parse(fps.Groups[1].Value),
            Bitrate = (long)float.Parse(bitrate.Groups[1].Value) * 1000L,
            Scale = float.Parse(speed.Groups[1].Value)
        };

        return true;
    }
}