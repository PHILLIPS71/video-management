using Giantnodes.Infrastructure.Domain.Entities;

namespace Giantnodes.Service.Dashboard.Domain.Entities.Files.Streams;

public abstract class FileSystemFileStream : IEntity, ITimestampableEntity
{
    public Guid Id { get; set; }

    public Guid FileSystemFileId { get; set; }
    public FileSystemFile? FileSystemFile { get; set; }

    public required int Index { get; set; }

    public required string Codec { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime CreatedAt { get; set; }
}