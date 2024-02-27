using Giantnodes.Service.Dashboard.Domain.Aggregates.EncodeProfiles;
using Giantnodes.Service.Dashboard.Domain.Enumerations;
using MassTransit;

namespace Giantnodes.Service.Dashboard.Persistence.Sagas;

public class EncodeSagaState : SagaStateMachineInstance
{
    public Guid CorrelationId { get; set; }

    public string CurrentState { get; set; } = null!;

    public Guid EncodeId { get; set; }

    public Guid? JobId { get; set; }

    public string InputFilePath { get; set; } = null!;

    public string OutputFilePath { get; set; } = null!;

    public EncodeCodec Codec { get; set; } = null!;

    public EncodePreset Preset { get; set; } = null!;

    public EncodeTune? Tune { get; set; }
    
    public int? Quality { get; set; }

    public bool IsKeepingSourceFile { get; set; }

    public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;

    public byte[]? RowVersion { get; set; }
}