namespace Giantnodes.Service.Dashboard.Domain.Aggregates.Libraries.Values;

public class SubtitleStream : FileStream
{
    public string Language { get; init; }

    public string? Title { get; init; }

    private SubtitleStream()
    {
    }

    public SubtitleStream(int index, string codec)
        : base(index, codec)
    {
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Index;
        yield return Codec;
        yield return Language;
        yield return Title;
    }
}