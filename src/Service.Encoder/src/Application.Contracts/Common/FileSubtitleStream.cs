namespace Giantnodes.Service.Encoder.Application.Contracts.Common;

public sealed record FileSubtitleStream
{
    public required int Index { get; init; }

    public required string Codec { get; init; }

    public required string Language { get; init; }

    public string? Title { get; init; }

    public int? Default { get; init; }

    public int? Forced { get; init; }
}