using MassTransit;

namespace Giantnodes.Service.Dashboard.Persistence.Sagas;

public class EncodeSaga : SagaStateMachineInstance, ISagaVersion
{
    public required Guid CorrelationId { get; set; }

    public required string CurrentState { get; set; }

    public required string FullPath { get; set; }
    
    public Guid JobId { get; set; }

    public DateTime SubmittedAt { get; set; }

    public DateTime? StartedAt { get; set; }

    public DateTime? CancelledAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public byte[]? RowVersion { get; set; }

    public int Version { get; set; }
}