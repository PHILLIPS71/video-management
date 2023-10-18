namespace Giantnodes.Service.Dashboard.Domain.Aggregates.Libraries.Values;

public class AudioStream : FileStream
{
    public string? Title { get; init; }

    public string? Language { get; init; }

    public required TimeSpan Duration { get; init; }

    public required long Bitrate { get; init; }

    public required int SampleRate { get; init; }

    public required int Channels { get; init; }

    private AudioStream()
    {
    }

    public AudioStream(int index, string codec)
        : base(index, codec)
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