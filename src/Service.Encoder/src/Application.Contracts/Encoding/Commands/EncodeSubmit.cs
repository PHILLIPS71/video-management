using MassTransit;

namespace Giantnodes.Service.Encoder.Application.Contracts.Encoding.Commands;

public sealed class EncodeSubmit
{
    public sealed record Command : CorrelatedBy<Guid>
    {
        public required Guid CorrelationId { get; init; }

        public required string FilePath { get; init; }

        public required bool IsDeletingInput { get; init; }

        public required string? OutputDirectoryPath { get; init; }
    }

    public sealed record Result : CorrelatedBy<Guid>
    {
        public required Guid CorrelationId { get; init; }
    }
}