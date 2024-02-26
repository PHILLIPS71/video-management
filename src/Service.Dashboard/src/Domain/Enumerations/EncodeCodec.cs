using Giantnodes.Infrastructure;

namespace Giantnodes.Service.Dashboard.Domain.Enumerations;

public sealed class EncodeCodec : Enumeration
{
    public string Description { get; init; }

    private EncodeCodec(int id, string name)
        : base(id, name)
    {
    }

    public EncodeCodec(int id, string name, string description)
        : base(id, name)
    {
        Description = description;
    }

    public static readonly EncodeCodec H264 = new(1, "H.264",
        "H.264 offers a balance between compression efficiency and playback performance");

    public static readonly EncodeCodec H265 = new(1, "H.265",
        "H.265 offers improved compression efficiency, making it suitable for high-resolution content like 4K and HDR video streaming, maintaining quality at lower bitrates");

    public static readonly EncodeCodec Vp9 = new(1, "VP9",
        "VP9 offers efficient compression and is commonly used in web browsers for streaming high-quality video content");

    public static readonly EncodeCodec Av1 = new(1, "AV1",
        "AV1 offers superior compression efficiency due to its ability to deliver high-quality video at lower bitrates, making it ideal for streaming and online video distribution");
}