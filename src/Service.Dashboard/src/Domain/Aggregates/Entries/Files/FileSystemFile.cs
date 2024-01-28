using System.IO.Abstractions;
using Giantnodes.Service.Dashboard.Domain.Aggregates.Encodes;
using Giantnodes.Service.Dashboard.Domain.Aggregates.Entries.Directories;
using Giantnodes.Service.Dashboard.Domain.Aggregates.Libraries;
using Giantnodes.Service.Dashboard.Domain.Values;
using FileStream = Giantnodes.Service.Dashboard.Domain.Values.FileStream;

namespace Giantnodes.Service.Dashboard.Domain.Aggregates.Entries.Files;

public class FileSystemFile : FileSystemEntry
{
    private readonly List<Encode> _encodes = new();

    public DateTime? ProbedAt { get; private set; }

    public IReadOnlyCollection<VideoStream> VideoStreams { get; private set; } = new List<VideoStream>();

    public IReadOnlyCollection<AudioStream> AudioStreams { get; private set; } = new List<AudioStream>();

    public IReadOnlyCollection<SubtitleStream> SubtitleStreams { get; private set; } = new List<SubtitleStream>();
    
    public IReadOnlyCollection<Encode> Encodes { get; private set; }

    private FileSystemFile()
    {
        Encodes = _encodes;
    }

    public FileSystemFile(Library library, FileSystemDirectory parent, IFileInfo file)
        : base(library, parent, file)
    {
        Size = file.Length;
        Encodes = _encodes;
    }

    public void SetSize(IFileInfo file)
    {
        if (file.FullName != PathInfo.FullName)
            throw new ArgumentException("the file info path provided does not match the file system entry");

        Size = file.Length;
    }

    public void SetStreams(DateTime timestamp, params FileStream[] streams)
    {
        VideoStreams = VideoStreams
            .Union(streams.OfType<VideoStream>())
            .Intersect(streams.OfType<VideoStream>())
            .ToList();

        AudioStreams = AudioStreams
            .Union(streams.OfType<AudioStream>())
            .Intersect(streams.OfType<AudioStream>())
            .ToList();

        SubtitleStreams = SubtitleStreams
            .Union(streams.OfType<SubtitleStream>())
            .Intersect(streams.OfType<SubtitleStream>())
            .ToList();

        ProbedAt = timestamp.ToUniversalTime();
    }

    public Encode Encode()
    {
        var encode = new Encode(this);
        _encodes.Add(encode);

        return encode;
    }
}