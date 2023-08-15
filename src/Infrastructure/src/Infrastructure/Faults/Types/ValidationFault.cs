using Giantnodes.Infrastructure.Validation.Contracts;

namespace Giantnodes.Infrastructure.Faults.Types;

public sealed record ValidationFault : IFault
{
    /// <inheritdoc />
    public required Guid? RequestId { get; init; }

    /// <inheritdoc />
    public required FaultType Type { get; init; }

    /// <inheritdoc />
    public required string Code { get; init; }

    /// <inheritdoc />
    public required string Message { get; init; }

    /// <inheritdoc />
    public required DateTime TimeStamp { get; init; }

    /// <summary>
    /// A collection of invalid properties that caused the fault to occur.
    /// </summary>
    public IEnumerable<InvalidValidationProperty> Properties { get; init; } = Enumerable.Empty<InvalidValidationProperty>();
}