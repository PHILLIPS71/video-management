namespace Giantnodes.Service.Dashboard.Domain.Entities.Files.Streams;

public class FileSystemFileVideoStream : FileSystemFileStream
{
    public required long Bitrate { get; set; }
    
    public required double Framerate { get; set; }

    public required string PixelFormat { get; set; }

    public required int Height { get; set; }

    public required int Width { get; set; }

    public required TimeSpan Duration { get; set; }

    public required string Ratio { get; set; }

    public int? Rotation { get; set; }

    public bool Default { get; set; }

    public bool Forced { get; set; }
}