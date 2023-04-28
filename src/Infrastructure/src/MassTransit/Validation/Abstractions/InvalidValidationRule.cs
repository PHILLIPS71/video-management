namespace Giantnodes.Infrastructure.MassTransit.Validation.Abstractions
{
    public sealed record InvalidValidationRule
    {
        public required string Rule { get; init; }

        public required string Reason { get; init; }
    }
}
