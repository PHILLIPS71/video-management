using Giantnodes.Infrastructure.Domain.Events;

namespace Giantnodes.Service.Encoder.Application.Contracts.Encoding.Events;

public sealed record EncodeOperationEncodeBuiltEvent : IntegrationEvent
{
    public required Guid JobId { get; init; }

    public required string FFmpegCommand { get; init; }

    public required string MachineName { get; init; }

    public required string MachineUserName { get; init; }
}