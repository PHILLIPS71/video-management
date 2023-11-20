namespace Giantnodes.Service.Dashboard.Domain.Shared.Enums;

public enum TranscodeStatus
{
    Submitted,
    Queued,
    Transcoding,
    Completed,
    Cancelling,
    Cancelled,
    Degraded,
    Failed
}