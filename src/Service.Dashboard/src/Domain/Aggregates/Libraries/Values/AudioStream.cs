namespace Giantnodes.Service.Dashboard.Domain.Aggregates.Libraries.Values;

public class AudioStream : FileStream
{
    public string? Title { get; init; }

    public string? Language { get; init; }

    public TimeSpan Duration { get; init; }

    public long Bitrate { get; init; }

    public int SampleRate { get; init; }

    public int Channels { get; init; }

    private AudioStream()
    {
    }

    public AudioStream(int index, string codec, string? title, string? language, TimeSpan duration, long bitrate, int sampleRate, int channels)
        : base(index, codec)
    {
        Title = title;
        Language = language;
        Duration = duration;
        Bitrate = bitrate;
        SampleRate = sampleRate;
        Channels = channels;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Index;
        yield return Codec;
        yield return Duration;
        yield return Bitrate;
        yield return SampleRate;
        yield return Channels;
    }
}