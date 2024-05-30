using Giantnodes.Infrastructure;

namespace Giantnodes.Service.Dashboard.Domain.Enumerations;

public sealed record EncodeTune : Enumeration
{
    public string Value { get; init; }

    public string Description { get; init; }

    private EncodeTune()
        : base(0, string.Empty)
    {
    }

    private EncodeTune(int id, string name)
        : base(id, name)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="EncodeTune"/> class with specified parameters.
    /// </summary>
    /// <param name="id">The ID of the tune.</param>
    /// <param name="name">The name of the tune.</param>
    /// <param name="value">The ffmpeg value associated with the tune.</param>
    /// <param name="description">The description of the tune.</param>
    private EncodeTune(int id, string name, string value, string description)
        : base(id, name)
    {
        Value = value;
        Description = description;
    }

    public static readonly EncodeTune Film = new(1, "Film", "film",
        "Suitable for high-quality film content, it optimizes for content with a lot of details and typically lower motion");

    public static readonly EncodeTune Animation = new(2, "Animation", "animation",
        "Suitable for animated content, it optimizes for flat areas and sharp lines commonly found in cartoons and animations");

    public static readonly EncodeTune Grain = new(3, "Grain", "grain",
        "Suitable for content with film grain, optimizing the encoding for preserving grain details");

    public static readonly EncodeTune StillImage = new(4, "Still Image", "stillimage",
        "Tailored for still images or slideshows where each frame is independent");

    public static readonly EncodeTune FastDecode = new(5, "Fast Decode", "fastdecode",
        "Optimized for fast decoding, sacrificing some compression efficiency");

    public static readonly EncodeTune ZeroLatency =
        new(6, "Zero Latency", "zerolatency",
            "Suitable for real-time streaming applications where low latency is essential");
}