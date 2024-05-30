using Giantnodes.Infrastructure;

namespace Giantnodes.Service.Dashboard.Domain.Enumerations;

public sealed record EncodePreset : Enumeration
{
    public string Value { get; init; }

    public string Description { get; init; }

    private EncodePreset()
        : base(0, string.Empty)
    {
    }

    private EncodePreset(int id, string name)
        : base(id, name)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="EncodePreset"/> class with specified parameters.
    /// </summary>
    /// <param name="id">The ID of the preset.</param>
    /// <param name="name">The name of the preset.</param>
    /// <param name="value">The ffmpeg value associated with the preset.</param>
    /// <param name="description">The description of the preset.</param>
    private EncodePreset(int id, string name, string value, string description)
        : base(id, name)
    {
        Value = value;
        Description = description;
    }

    public static readonly EncodePreset VerySlow = new(1, "Very Slow", "veryslow",
        "Critical scenarios where the absolute best quality is required, regardless of time");

    public static readonly EncodePreset Slower = new(2, "Slower", "slower",
        "Enhanced compression efficiency with a slower encoding process");

    public static readonly EncodePreset Slow = new(3, "Slow", "slow",
        "Ideal for high-quality video production where quality matters more than speed");

    public static readonly EncodePreset Medium = new(4, "Medium", "medium",
        "Well-balanced preset for general video encoding needs");

    public static readonly EncodePreset Fast = new(5, "Fast", "fast",
        "A moderate balance between speed and compression, suitable for various applications");

    public static readonly EncodePreset Faster = new(6, "Faster", "faster",
        "Prioritizes compression over speed, better for smaller file sizes");

    public static readonly EncodePreset VeryFast = new(7, "Very Fast", "veryfast",
        "A good compromise between speed and compression, suitable for online streaming or quick video sharing");

    public static readonly EncodePreset SuperFast = new(8, "Super Fast", "superfast",
        "Very fast with a slight balance of speed and compression, suitable for quick transcoding");

    public static readonly EncodePreset UltraFast = new(9, "Ultra Fast", "ultrafast",
        "Extremely fast but sacrifices compression efficiency, ideal for live streaming");
}