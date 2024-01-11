using MassTransit;

namespace Giantnodes.Service.Encoder.Persistence.Sagas;

public class EncodeJobSaga : SagaStateMachineInstance
{
    public Guid CorrelationId { get; set; }

    public string CurrentState { get; set; } = null!;

    public Guid? JobId { get; set; }

    public string InputPath { get; set; } = null!;

    public bool IsDeletingInput { get; set; }

    public string? OutputTempPath { get; set; }

    public string? OutputDirectoryPath { get; set; }

    public byte[]? RowVersion { get; set; }
}