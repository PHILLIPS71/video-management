using Giantnodes.Infrastructure.Messages;
using MassTransit;

namespace Giantnodes.Service.Encoder.Application.Contracts.Encoding.Commands;

public sealed class EncodeCancel
{
    public sealed record Command : Message
    {
    }

    public sealed record Result : CorrelatedBy<Guid>
    {
        public required Guid CorrelationId { get; init; }
    }
}