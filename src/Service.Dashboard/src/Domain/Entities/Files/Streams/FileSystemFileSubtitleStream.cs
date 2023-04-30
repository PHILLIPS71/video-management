namespace Giantnodes.Service.Dashboard.Domain.Entities.Files.Streams;

public class FileSystemFileSubtitleStream : FileSystemFileStream
{
    public string? Title { get; set; }

    public required string Language { get; set; }
    
    public bool Default { get; set; }

    public bool Forced { get; set; }
}