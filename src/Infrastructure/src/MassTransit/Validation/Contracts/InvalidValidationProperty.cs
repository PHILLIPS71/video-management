namespace Giantnodes.Infrastructure.MassTransit.Validation.Contracts
{
    public sealed record InvalidValidationProperty
    {
        public required string Property { get; init; }

        public ICollection<InvalidValidationRule> Issues { get; set; } = new List<InvalidValidationRule>();
    }
}
