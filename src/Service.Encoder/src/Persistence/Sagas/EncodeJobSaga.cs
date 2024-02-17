using MassTransit;

namespace Giantnodes.Service.Encoder.Persistence.Sagas;

public class EncodeJobSaga : SagaStateMachineInstance
{
    public Guid CorrelationId { get; set; }

    public string CurrentState { get; set; } = null!;

    public Guid? JobId { get; set; }

    public string FilePath { get; set; } = null!;

    public bool IsDeletingInput { get; set; }

    public string? OutputTempFilePath { get; set; }

    public string? OutputDirectoryPath { get; set; }

    public byte[]? RowVersion { get; set; }
}