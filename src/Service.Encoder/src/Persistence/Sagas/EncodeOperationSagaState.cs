using MassTransit;

namespace Giantnodes.Service.Encoder.Persistence.Sagas;

public class EncodeOperationSagaState : SagaStateMachineInstance
{
    public Guid CorrelationId { get; set; }

    public string CurrentState { get; set; } = null!;

    public Guid? JobId { get; set; }

    public string InputFilePath { get; set; } = null!;

    public string OutputFilePath { get; set; } = null!;

    public string TempFilePath { get; set; } = null!;

    public byte[]? RowVersion { get; set; }
}