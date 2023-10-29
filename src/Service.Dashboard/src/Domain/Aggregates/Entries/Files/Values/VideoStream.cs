namespace Giantnodes.Service.Dashboard.Domain.Aggregates.Entries.Files.Values;

public class VideoStream : FileStream
{
    public TimeSpan Duration { get; init; }

    public VideoQuality Quality { get; init; }

    public double Framerate { get; init; }

    public long Bitrate { get; init; }

    public string PixelFormat { get; init; }

    private VideoStream()
    {
    }

    public VideoStream(
        int index,
        string codec,
        TimeSpan duration,
        VideoQuality quality,
        double framerate,
        long bitrate,
        string pixelFormat) : base(index, codec)
    {
        Duration = duration;
        Quality = quality;
        Framerate = framerate;
        Bitrate = bitrate;
        PixelFormat = pixelFormat;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Index;
        yield return Codec;
        yield return Duration;
        yield return Quality;
        yield return Framerate;
        yield return Bitrate;
        yield return PixelFormat;
    }
}