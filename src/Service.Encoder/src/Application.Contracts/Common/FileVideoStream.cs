namespace Giantnodes.Service.Encoder.Application.Contracts.Common;

public sealed record FileVideoStream
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

    public int? Rotation { get; init; }

    public int? Default { get; init; }

    public int? Forced { get; init; }
}