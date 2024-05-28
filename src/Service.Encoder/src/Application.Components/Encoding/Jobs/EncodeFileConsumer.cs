using System.Diagnostics;
using System.IO.Abstractions;
using System.Text.RegularExpressions;
using Giantnodes.Infrastructure.Faults;
using Giantnodes.Service.Encoder.Application.Contracts.Encoding.Events;
using Giantnodes.Service.Encoder.Application.Contracts.Encoding.Jobs;
using MassTransit;
using Microsoft.Extensions.Logging;
using Polly;
using Xabe.FFmpeg;
using Xabe.FFmpeg.Events;
using Xabe.FFmpeg.Exceptions;

namespace Giantnodes.Service.Encoder.Application.Components.Encoding.Jobs;

public class EncodeFileConsumer : IJobConsumer<EncodeFile.Job>
{
    private readonly IFileSystem _fs;
    private readonly ILogger<EncodeFileConsumer> _logger;

    public EncodeFileConsumer(IFileSystem fs, ILogger<EncodeFileConsumer> logger)
    {
        _fs = fs;
        _logger = logger;
    }

    public async Task Run(JobContext<EncodeFile.Job> context)
    {
        var file = _fs.FileInfo.New(context.Job.InputFilePath);
        if (!file.Exists)
        {
            await context.RejectAsync(FaultKind.NotFound, nameof(context.Job.InputFilePath));
            return;
        }

        try
        {
            var accelerated = context.Job.UseHardwareAcceleration;

            await Policy<IConversionResult>
                .Handle<ConversionException>(_=> accelerated)
                .RetryAsync(1, onRetry: (result, count, ctx) =>
                {
                    _logger.LogWarning("encode {JobId} encountered a conversion exception and will retry without hardware acceleration", context.JobId);
                    accelerated = false;
                })
                .ExecuteAsync(async () =>
                {
                    var conversion = await context.ToConversion(file, accelerated, _logger);
                    var @event = new EncodeOperationEncodeBuiltEvent
                    {
                        JobId = context.JobId,
                        CorrelationId = context.Job.CorrelationId,
                        FFmpegCommand = conversion.Build(),
                        MachineName = Environment.MachineName,
                        MachineUserName = Environment.UserName,
                        UsingHardwareAcceleration = accelerated
                    };

                    await context.Publish(@event, context.CancellationToken);
                    return await conversion.Start();
                });
        }
        catch (OperationCanceledException ex)
        {
            var info = _fs.FileInfo.New(context.Job.OutputFilePath);
            if (!info.Exists)
                throw;

            info.Delete();
            _logger.LogInformation(ex, "encode {JobId} was cancelled and file {FileName} was deleted.", context.JobId, info.FullName);
            throw;
        }
    }
}

internal static class JobContextExtensions
{
    internal static async Task<IConversion> ToConversion(
        this JobContext<EncodeFile.Job> context,
        IFileInfo file,
        bool accelerate,
        ILogger<EncodeFileConsumer> logger)
    {
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

        if (accelerate)
        {
            var decoder = media.VideoStreams.First().Codec;
            var encoder = context.Job.Codec;

            conversion = conversion
                .UseHardwareAcceleration(nameof(HardwareAccelerator.auto), decoder, encoder);
        }

        conversion.OnDataReceived += async (_, args) =>
        {
            if (string.IsNullOrWhiteSpace(args.Data))
                return;

            EncodeOperationOutputtedEvent.ConversionSpeed? speed = null;
            try
            {
                speed = args.ToSpeed();
            }
            catch (FormatException ex)
            {
                logger.LogError(ex, "encode {JobId} was unable to parse output: {Data}.", context.JobId, args.Data);
            }

            var @event = new EncodeOperationOutputtedEvent
            {
                JobId = context.JobId,
                CorrelationId = context.Job.CorrelationId,
                Output = args.Data,
                Speed = speed
            };

            await context.Publish(@event, context.CancellationToken);
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

            await context.Publish(@event, context.CancellationToken);

            progress = args;
            logger.LogInformation("encode {JobId} on file {FilePath} progressed to {Percent:P}.", context.JobId, context.Job.OutputFilePath, args.Percent / 100.0f);
        };

        return conversion;
    }
}

internal static partial class DataReceivedEventArgsExtensions
{
    [GeneratedRegex("bitrate=[ ]*([+-]?[0-9]*[.]?[0-9]*)")]
    private static partial Regex BitrateRegex();

    [GeneratedRegex("fps=[ ]*([+-]?[0-9]*[.]?[0-9]*)")]
    private static partial Regex FpsRegex();

    [GeneratedRegex("speed=[ ]*([+-]?[0-9]*[.]?[0-9]*)x")]
    private static partial Regex SpeedRegex();

    internal static EncodeOperationOutputtedEvent.ConversionSpeed? ToSpeed(this DataReceivedEventArgs args)
    {
        if (string.IsNullOrWhiteSpace(args.Data) || args.Data.Contains("N/A"))
            return null;
        
        var fps = FpsRegex().Match(args.Data);
        if (!fps.Success)
            return null;

        var bitrate = BitrateRegex().Match(args.Data);
        if (!bitrate.Success)
            return null;

        var speed = SpeedRegex().Match(args.Data);
        if (!speed.Success)
            return null;

        return new EncodeOperationOutputtedEvent.ConversionSpeed
        {
            Frames = float.Parse(fps.Groups[1].Value),
            Bitrate = (long)float.Parse(bitrate.Groups[1].Value) * 1000L,
            Scale = float.Parse(speed.Groups[1].Value)
        };
    }
}