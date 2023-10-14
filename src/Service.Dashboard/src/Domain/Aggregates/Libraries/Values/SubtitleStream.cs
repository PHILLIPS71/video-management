using Giantnodes.Infrastructure.Domain.Values;

namespace Giantnodes.Service.Dashboard.Domain.Aggregates.Libraries.Values;

public class SubtitleStream : ValueObject
{
    public required int Index { get; init; }

    public required string Codec { get; init; }

    public required string Language { get; init; }

    public string? Title { get; init; }

    public bool Default { get; init; }

    public bool Forced { get; init; }

    private SubtitleStream()
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