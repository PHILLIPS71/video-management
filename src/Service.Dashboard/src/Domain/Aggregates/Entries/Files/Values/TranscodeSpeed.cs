using Giantnodes.Infrastructure.Domain.Values;

namespace Giantnodes.Service.Dashboard.Domain.Aggregates.Entries.Files.Values;

public class TranscodeSpeed : ValueObject
{
    public float Frames { get; set; }

    public long Bitrate { get; set; }

    public float Scale { get; set; }

    protected TranscodeSpeed()
    {
    }

    public TranscodeSpeed(float fps, long bitrate, float scale)
    {
        Frames = fps;
        Bitrate = bitrate;
        Scale = scale;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Frames;
        yield return Bitrate;
        yield return Scale;
    }
}