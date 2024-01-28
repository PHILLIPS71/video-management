namespace Giantnodes.Service.Dashboard.Domain.Values;

public class SubtitleStream : FileStream
{
    public string? Title { get; init; }

    public string? Language { get; init; }

    private SubtitleStream()
    {
    }

    public SubtitleStream(int index, string codec, string? title, string? language)
        : base(index, codec)
    {
        Title = title;
        Language = language;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Index;
        yield return Codec;
    }
}