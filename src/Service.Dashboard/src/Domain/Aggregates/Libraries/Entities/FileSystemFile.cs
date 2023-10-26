using System.IO.Abstractions;
using Giantnodes.Service.Dashboard.Domain.Aggregates.Libraries.Values;
using FileStream = Giantnodes.Service.Dashboard.Domain.Aggregates.Libraries.Values.FileStream;

namespace Giantnodes.Service.Dashboard.Domain.Aggregates.Libraries.Entities;

public class FileSystemFile : FileSystemEntry
{
    public IReadOnlyCollection<VideoStream> VideoStreams { get; private set; } = new List<VideoStream>();

    public IReadOnlyCollection<AudioStream> AudioStreams { get; private set; } = new List<AudioStream>();

    public IReadOnlyCollection<SubtitleStream> SubtitleStreams { get; private set; } = new List<SubtitleStream>();

    protected FileSystemFile()
    {
    }

    public FileSystemFile(FileSystemDirectory parent, IFileInfo file)
        : base(parent, file)
    {
        Size = file.Length;
    }

    public void SetSize(IFileInfo file)
    {
        if (file.FullName != PathInfo.FullName)
            throw new ArgumentException("the file info path provided does not match the file system entry");

        Size = file.Length;
    }

    public void SetStreams(params FileStream[] streams)
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
    }
}