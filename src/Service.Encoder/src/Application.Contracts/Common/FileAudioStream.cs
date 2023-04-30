namespace Giantnodes.Service.Encoder.Application.Contracts.Common;

public sealed record FileAudioStream
{
    public required int Index { get; init; }

    public required string Codec { get; init; }

    public string? Title { get; init; }

    public string? Language { get; init; }
    
    public required TimeSpan Duration { get; init; }

    public required long Bitrate { get; init; }

    public required int SampleRate { get; init; }

    public required int Channels { get; init; }

    public required int? Default { get; init; }

    public required int? Forced { get; init; }
}