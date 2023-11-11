namespace Giantnodes.Service.Dashboard.Application.Contracts.Transcoding.Commands;

public sealed class FileTranscode
{
    public sealed record Command
    {
        public required Guid FileId { get; init; }
    }

    public sealed record Result
    {
        public required Guid TranscodeId { get; init; }
    }
}