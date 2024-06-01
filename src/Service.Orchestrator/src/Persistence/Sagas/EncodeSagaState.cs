using MassTransit;

namespace Giantnodes.Service.Orchestrator.Persistence.Sagas;

public class EncodeSagaState : SagaStateMachineInstance
{
    /// <summary>
    /// Gets or sets the correlation identifier for this saga instance.
    /// </summary>
    public Guid CorrelationId { get; set; }

    /// <summary>
    /// Gets or sets the current state of the saga.
    /// </summary>
    public string CurrentState { get; set; } = null!;

    /// <summary>
    /// Gets or sets the identifier of the encode aggregate.
    /// </summary>
    public Guid EncodeId { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the file probe job.
    /// </summary>
    public Guid? JobId { get; set; }

    /// <summary>
    /// Gets or sets the path to the input file being encoded.
    /// </summary>
    public string InputFilePath { get; set; } = null!;

    /// <summary>
    /// Gets or sets the path to the output location of the encoded file.
    /// </summary>
    public string OutputFilePath { get; set; } = null!;

    /// <summary>
    /// Gets or sets a value indicating whether the source file should be kept after the process completes.
    /// </summary>
    public bool IsKeepingSourceFile { get; set; }

    /// <summary>
    /// Gets or sets the reason for a failure.
    /// </summary>
    public string? FailedReason { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the encode was submitted.
    /// </summary>
    public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Gets or sets the row version for concurrency control.
    /// </summary>
    public byte[]? RowVersion { get; set; }
}