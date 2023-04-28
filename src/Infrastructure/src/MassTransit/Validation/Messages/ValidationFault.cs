using Giantnodes.Infrastructure.MassTransit.Validation.Abstractions;

namespace Giantnodes.Infrastructure.MassTransit.Validation.Messages
{
    public sealed record ValidationFault
    {
        public InvalidValidationProperty[] Properties { get; init; } = Array.Empty<InvalidValidationProperty>();
    }
}
