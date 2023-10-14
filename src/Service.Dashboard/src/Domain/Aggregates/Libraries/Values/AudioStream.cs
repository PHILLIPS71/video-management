using Giantnodes.Infrastructure.Domain.Values;

namespace Giantnodes.Service.Dashboard.Domain.Aggregates.Libraries.Values;

public class AudioStream : ValueObject
{
    public required int Index { get; init; }

    public required string Codec { get; init; }

    public string? Title { get; init; }

    public string? Language { get; init; }

    public required TimeSpan Duration { get; init; }

    public required long Bitrate { get; init; }

    public required int SampleRate { get; init; }

    public required int Channels { get; init; }

    public bool Default { get; init; }

    public bool Forced { get; init; }

    private AudioStream()
    {
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Index;
        yield return Codec;
        yield return Title;
        yield return Language;
        yield return Duration;
        yield return Bitrate;
        yield return SampleRate;
        yield return Channels;
    }
}