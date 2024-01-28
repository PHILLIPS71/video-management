namespace Giantnodes.Service.Dashboard.Application.Contracts.Encodes.Events;

public sealed record EncodeCreatedEvent
{
    public required Guid FileId { get; init; }

    public required Guid EncodeId { get; init; }

    public required string FullPath { get; init; }
}