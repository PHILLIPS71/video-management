using Giantnodes.Infrastructure;

namespace Giantnodes.Service.Dashboard.Domain.Enumerations;

public sealed record EncodeCodec : Enumeration
{
    /// <summary>
    /// Initializes a new instance of the <see cref="EncodeQuality"/> struct with specified parameters.
    /// </summary>
    /// <param name="Min">The minimum quality value.</param>
    /// <param name="Max">The maximum quality value.</param>
    /// <param name="Default">The default quality value.</param>
    public readonly record struct EncodeQuality(int Min, int Max, int Default);

    public string Value { get; init; }

    public string Description { get; init; }

    public IEnumerable<EncodeTune> Tunes { get; init; }

    public EncodeQuality Quality { get; init; }

    private EncodeCodec()
        : base(0, string.Empty)
    {
    }

    private EncodeCodec(int id, string name)
        : base(id, name)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="EncodeCodec"/> class with specified parameters.
    /// </summary>
    /// <param name="id">The ID of the codec.</param>
    /// <param name="name">The name of the codec.</param>
    /// <param name="value">The ffmpeg value associated with the codec.</param>
    /// <param name="description">The description of the codec.</param>
    /// <param name="tunes">The collection of encoding tunes supported by the codec.</param>
    /// <param name="quality">The quality settings for the codec.</param>
    private EncodeCodec(int id, string name, string value, string description, IEnumerable<EncodeTune> tunes, EncodeQuality quality)
        : base(id, name)
    {
        Value = value;
        Description = description;
        Tunes = tunes;
        Quality = quality;
    }

    public static readonly EncodeCodec H264 = new(
        1,
        "H.264",
        "h264",
        "H.264 offers a balance between compression efficiency and playback performance",
        GetAll<EncodeTune>(),
        new EncodeQuality(0, 51, 23));

    public static readonly EncodeCodec H265 = new(
        2,
        "H.265",
        "hevc",
        "H.265 offers improved compression efficiency, making it suitable for high-resolution content like 4K and HDR video streaming, maintaining quality at lower bitrates",
        new[] { EncodeTune.FastDecode, EncodeTune.Grain, EncodeTune.ZeroLatency },
        new EncodeQuality(0, 51, 28));

    public static readonly EncodeCodec Vp9 = new(
        3,
        "VP9",
        "vp9",
        "VP9 offers efficient compression and is commonly used in web browsers for streaming high-quality video content",
        Array.Empty<EncodeTune>(),
        new EncodeQuality(0, 63, 31));

    public static readonly EncodeCodec Av1 = new(
        4,
        "AV1",
        "av1",
        "AV1 offers superior compression efficiency due to its ability to deliver high-quality video at lower bitrates, making it ideal for streaming and online video distribution",
        new[] { EncodeTune.FastDecode },
        new EncodeQuality(0, 63, 30));
}