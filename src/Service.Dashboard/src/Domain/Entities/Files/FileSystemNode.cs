using Giantnodes.Infrastructure.Domain.Entities;

namespace Giantnodes.Service.Dashboard.Domain.Entities.Files;

public abstract class FileSystemNode : IEntity, ITimestampableEntity
{
    public Guid Id { get; set; }

    public string FullPath { get; set; } = string.Empty;
    
    public string Name { get; set; } = string.Empty;
    
    public Guid? ParentDirectoryId { get; set; }
    public FileSystemDirectory? ParentDirectory { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime CreatedAt { get; set; }
}