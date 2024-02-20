using Giantnodes.Infrastructure.Messages;

namespace Giantnodes.Service.Encoder.Application.Contracts.Encoding.Commands;

public sealed class FileTransfer
{
    public sealed record Job : Message
    {
        public required string InputFilePath { get; init; }

        public required string OutputDirectoryPath { get; set; }

        public string? FileName { get; init; }
    }

    public sealed record Result
    {
        public required string FilePath { get; init; }
    }
}