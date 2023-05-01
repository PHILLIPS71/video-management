namespace Giantnodes.Service.Encoder.Application.Contracts.Encoding.Jobs;

public sealed record EncodeFile
{
    public required string FullPath { get; init; }

    public required string Preset { get; init; }

    public string? Container { get; init; }

    public required EncodeFileVideoStream[] VideoStreams { get; init; }

    public EncodeFileAudioStream[] AudioStreams { get; init; } = Array.Empty<EncodeFileAudioStream>();
}

public sealed record EncodeFileVideoStream
{
    public required string Codec { get; init; }

    public int? Height { get; init; }

    public int? Width { get; init; }

    public long? Bitrate { get; init; }

    public double? Framerate { get; init; }
}

public sealed record EncodeFileAudioStream
{
    public required string Codec { get; init; }

    public int? Channels { get; init; }
    
    public long? Bitrate { get; init; }
}