namespace Giantnodes.Service.Dashboard.Domain.Shared.Enums;

public enum EncodeStatus
{
    Submitted,
    Queued,
    Encoding,
    Completed,
    Cancelling,
    Cancelled,
    Degraded,
    Failed
}