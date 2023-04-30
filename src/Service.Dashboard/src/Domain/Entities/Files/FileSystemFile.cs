using Giantnodes.Service.Dashboard.Domain.Entities.Files.Streams;

namespace Giantnodes.Service.Dashboard.Domain.Entities.Files;

public class FileSystemFile : FileSystemNode
{
    public required long Size { get; set; }
    
    public DateTime? ProbedAt { get; set; }
    
    public ICollection<FileSystemFileVideoStream>? VideoStreams { get; set; }

    public ICollection<FileSystemFileAudioStream>? AudioStreams { get; set; }

    public ICollection<FileSystemFileSubtitleStream>? SubtitleStreams { get; set; }
}