using Giantnodes.Infrastructure;

namespace Giantnodes.Service.Dashboard.Domain.Enumerations;

public sealed class EncodePreset : Enumeration
{
    public string Description { get; init; }

    private EncodePreset(int id, string name)
        : base(id, name)
    {
    }

    public EncodePreset(int id, string name, string description)
        : base(id, name)
    {
        Description = description;
    }

    public static readonly EncodePreset VerySlow = new(1, "Very Slow",
        "Critical scenarios where the absolute best quality is required, regardless of time");

    public static readonly EncodePreset Slower = new(2, "Slower",
        "Enhanced compression efficiency with a slower encoding process");

    public static readonly EncodePreset Slow = new(3, "Slow",
        "Ideal for high-quality video production where quality matters more than speed");

    public static readonly EncodePreset Medium = new(4, "Medium",
        "Well-balanced preset for general video encoding needs");

    public static readonly EncodePreset Fast = new(5, "Fast",
        "A moderate balance between speed and compression, suitable for various applications");

    public static readonly EncodePreset Faster = new(6, "Faster",
        "Prioritizes compression over speed, better for smaller file sizes");

    public static readonly EncodePreset VeryFast = new(7, "Very Fast",
        "A good compromise between speed and compression, suitable for online streaming or quick video sharing");

    public static readonly EncodePreset SuperFast = new(8, "Super Fast",
        "Very fast with a slight balance of speed and compression, suitable for quick transcoding");

    public static readonly EncodePreset UltraFast = new(9, "Ultra Fast",
        "Extremely fast but sacrifices compression efficiency, ideal for live streaming");
}