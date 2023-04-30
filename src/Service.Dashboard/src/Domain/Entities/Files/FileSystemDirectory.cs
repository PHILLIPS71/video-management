namespace Giantnodes.Service.Dashboard.Domain.Entities.Files;

public class FileSystemDirectory : FileSystemNode
{
    public virtual ICollection<FileSystemNode>? Nodes { get; set; }
}