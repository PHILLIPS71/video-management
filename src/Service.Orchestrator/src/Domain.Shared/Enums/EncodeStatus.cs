namespace Giantnodes.Service.Orchestrator.Domain.Shared.Enums;

public enum EncodeStatus
{
    Submitted,
    Queued,
    Encoding,
    Completed,
    Cancelled,
    Degraded,
    Failed
}