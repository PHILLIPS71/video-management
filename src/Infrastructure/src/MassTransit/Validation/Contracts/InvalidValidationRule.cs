namespace Giantnodes.Infrastructure.MassTransit.Validation.Contracts
{
    public sealed record InvalidValidationRule
    {
        public required string Rule { get; init; }

        public required string Reason { get; init; }
    }
}
