namespace Giantnodes.Service.Dashboard.Domain.Entities.Files.Streams;

public class FileSystemFileAudioStream : FileSystemFileStream
{
    public string? Title { get; set; }

    public string? Language { get; set; }
    
    public required TimeSpan Duration { get; set; }

    public required long Bitrate { get; set; }

    public required int SampleRate { get; set; }

    public required int Channels { get; set; }

    public bool Default { get; set; }

    public bool Forced { get; set; }
}