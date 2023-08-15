namespace Giantnodes.Infrastructure.Validation.Contracts;

public sealed record InvalidValidationProperty
{
    public required string Property { get; init; }

    public ICollection<InvalidValidationRule> Issues { get; init; } = new List<InvalidValidationRule>();
}