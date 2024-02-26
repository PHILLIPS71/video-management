using Giantnodes.Infrastructure;

namespace Giantnodes.Service.Dashboard.Domain.Enumerations;

public sealed class EncodeTune : Enumeration
{
    public string Description { get; init; }

    private EncodeTune(int id, string name)
        : base(id, name)
    {
    }

    public EncodeTune(int id, string name, string description)
        : base(id, name)
    {
        Description = description;
    }

    public static readonly EncodeTune Film = new(1, "Film", "use for high quality movie content; lowers de-blocking");

    public static readonly EncodeTune Animation = new(2, "Animation",
        "good for cartoons; uses higher de-blocking and more reference frames");

    public static readonly EncodeTune Grain = new(3, "Grain",
        "preserves the grain structure in old, grainy film material");

    public static readonly EncodeTune StillImage = new(4, "Still Image", "good for slideshow-like content");

    public static readonly EncodeTune FastDecode = new(5, "Fast Decode",
        "allows faster decoding by disabling certain filters");

    public static readonly EncodeTune ZeroLatency =
        new(6, "Zero Latency", " good for fast encoding and low-latency streaming");
}