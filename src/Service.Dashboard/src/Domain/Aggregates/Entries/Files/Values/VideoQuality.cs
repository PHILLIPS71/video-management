using Giantnodes.Infrastructure.Domain.Values;
using Giantnodes.Service.Dashboard.Domain.Enumerations;

namespace Giantnodes.Service.Dashboard.Domain.Aggregates.Entries.Files.Values;

public class VideoQuality : ValueObject
{
    public int Width { get; init; }

    public int Height { get; init; }

    public string AspectRatio { get; init; }

    public VideoResolution Resolution { get; init; }

    private VideoQuality()
    {
    }

    public VideoQuality(int width, int height, string aspectRatio)
    {
        Width = width;
        Height = height;
        AspectRatio = aspectRatio;
        Resolution = VideoResolution.FindResolution(width, height);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Width;
        yield return Height;
        yield return AspectRatio;
    }
}