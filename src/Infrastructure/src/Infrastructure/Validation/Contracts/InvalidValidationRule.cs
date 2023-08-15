namespace Giantnodes.Infrastructure.Validation.Contracts;

public sealed record InvalidValidationRule
{
    public required string Rule { get; init; }

    public required string Reason { get; init; }
}