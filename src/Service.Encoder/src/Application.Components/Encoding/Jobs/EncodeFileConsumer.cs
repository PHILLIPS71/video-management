using System.IO.Abstractions;
using Giantnodes.Service.Encoder.Application.Contracts.Encoding.Events;
using Giantnodes.Service.Encoder.Application.Contracts.Encoding.Jobs;
using MassTransit;
using Xabe.FFmpeg;
using Xabe.FFmpeg.Events;

namespace Giantnodes.Service.Encoder.Application.Components.Encoding.Jobs;

public class EncodeFileConsumer : IJobConsumer<EncodeFile>
{
    private readonly IFileSystem _system;

    public EncodeFileConsumer(IFileSystem system)
    {
        _system = system;
    }

    public async Task Run(JobContext<EncodeFile> context)
    {
        var file = _system.FileInfo.New(context.Job.FullPath);
        if (file.Exists == false)
            throw new FileNotFoundException($"Could not find a file at '{context.Job.FullPath}'.");

        var name = Path.ChangeExtension(context.JobId.ToString(), file.Extension);
        if (string.IsNullOrWhiteSpace(context.Job.Container) == false)
            name = Path.ChangeExtension(name, context.Job.Container);

        var output = Path.Join(@"C:\Users\jordy\OneDrive\Desktop\Conversions\Converted", name);

        var media = await FFmpeg.GetMediaInfo(file.FullName, context.CancellationToken);
        var conversion = FFmpeg.Conversions
            .New()
            .SetPreset(Enum.Parse<ConversionPreset>(context.Job.Preset))
            .SetOutput(output)
            .SetOverwriteOutput(true)
            .UseMultiThread(true);

        if (context.Job.VideoStreams.Any())
            conversion.AddStream(GetMappedVideoStreams(media, context.Job.VideoStreams));

        if (context.Job.AudioStreams.Any())
            conversion.AddStream(GetMappedAudioStreams(media, context.Job.AudioStreams));

        ConversionProgressEventArgs? progress = null;
        conversion.OnProgress += async (_, args) =>
        {
            if (progress?.Percent != args.Percent)
            {
                await context.Publish(new EncodeFileProgressed
                {
                    JobId = context.JobId,
                    Duration = args.Duration,
                    TotalLength = args.TotalLength,
                    Percent = args.Percent
                }, context.CancellationToken);
            }

            progress = args;
        };

        await conversion.Start(context.CancellationToken);
    }

    private static IEnumerable<IVideoStream> GetMappedVideoStreams(IMediaInfo media,
        IEnumerable<EncodeFileVideoStream> streams)
    {
        var videos = new List<IVideoStream>();
        foreach (var stream in streams)
        {
            var video = media
                .VideoStreams
                .FirstOrDefault(x => string.Equals(x.Codec, stream.Codec, StringComparison.InvariantCultureIgnoreCase));

            if (video == null)
                video = media.VideoStreams.First().SetCodec(stream.Codec);

            if (stream.Width.HasValue && stream.Height.HasValue)
                video.SetSize(stream.Width.Value, stream.Height.Value);

            if (stream.Bitrate.HasValue)
                video.SetBitrate(stream.Bitrate.Value);

            if (stream.Framerate.HasValue)
                video.SetFramerate(stream.Framerate.Value);

            videos.Add(video);
        }

        return videos;
    }

    private static IEnumerable<IAudioStream> GetMappedAudioStreams(IMediaInfo media,
        IEnumerable<EncodeFileAudioStream> streams)
    {
        var audios = new List<IAudioStream>();
        foreach (var stream in streams)
        {
            var audio = media
                .AudioStreams
                .OrderByDescending(x => x.Channels)
                .FirstOrDefault(x => string.Equals(x.Codec, stream.Codec, StringComparison.InvariantCultureIgnoreCase));

            if (audio == null)
                audio = media.AudioStreams.First().SetCodec(stream.Codec);

            if (stream.Channels.HasValue)
                audio.SetChannels(stream.Channels.Value);

            if (stream.Bitrate.HasValue)
                audio.SetBitrate(stream.Bitrate.Value);

            audios.Add(audio);
        }

        return audios;
    }
}