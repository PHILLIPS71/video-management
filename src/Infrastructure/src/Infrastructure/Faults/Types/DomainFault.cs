namespace Giantnodes.Infrastructure.Faults.Types;

public sealed record DomainFault : IFault
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
    /// A optional property that relates the the fault that occurred.
    /// </summary>
    public string? Property { get; init; }
}