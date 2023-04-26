using Giantnodes.Service.Dashboard.Domain.Entities.Files;

namespace Giantnodes.Service.Dashboard.Domain.Entities;

public class FileSystemDirectory : FileSystemNode
{
    public virtual ICollection<FileSystemNode>? Nodes { get; set; }
}