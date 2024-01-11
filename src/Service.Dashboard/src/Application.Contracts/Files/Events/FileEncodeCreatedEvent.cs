namespace Giantnodes.Service.Dashboard.Application.Contracts.Files.Events;

public sealed record FileEncodeCreatedEvent
{
    public required Guid FileId { get; init; }

    public required Guid EncodeId { get; init; }

    public required string FullPath { get; init; }
}