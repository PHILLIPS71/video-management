using MassTransit;

namespace Giantnodes.Service.Dashboard.Persistence.Sagas;

public class EncodeSagaState : SagaStateMachineInstance
{
    public Guid CorrelationId { get; set; }

    public string CurrentState { get; set; } = null!;

    public string InputFullPath { get; set; } = null!;

    public string? OutputFullPath { get; set; }

    public DateTime SubmittedAt { get; set; }

    public byte[]? RowVersion { get; set; }
}