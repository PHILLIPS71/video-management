using Giantnodes.Infrastructure.Domain.Values;

namespace Giantnodes.Service.Orchestrator.Domain.Aggregates.Encodes.Values;

public class EncodeSpeed : ValueObject
{
    public float Frames { get; init; }

    public long Bitrate { get; init; }

    public float Scale { get; init; }

    protected EncodeSpeed()
    {
    }

    public EncodeSpeed(float fps, long bitrate, float scale)
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