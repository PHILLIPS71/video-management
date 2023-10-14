using Giantnodes.Infrastructure.Domain.Values;

namespace Giantnodes.Service.Dashboard.Domain.Aggregates.Libraries.Values;

public class VideoStream : ValueObject
{
    public required int Index { get; init; }

    public required string Codec { get; init; }

    public required TimeSpan Duration { get; init; }

    public required int Width { get; init; }

    public required int Height { get; init; }

    public required double Framerate { get; init; }

    public required string Ratio { get; init; }

    public required long Bitrate { get; init; }

    public required string PixelFormat { get; init; }

    public bool Default { get; init; }

    public bool Forced { get; init; }

    public int? Rotation { get; init; }

    private VideoStream()
    {
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Index;
        yield return Codec;
        yield return Duration;
        yield return Width;
        yield return Height;
        yield return Framerate;
        yield return Ratio;
        yield return Bitrate;
        yield return PixelFormat;
    }
}