namespace Giantnodes.Service.Encoder.Application.Contracts.Encoding.Commands;

public sealed class FileTransfer
{
    public sealed record Command
    {
        public required string InputPath { get; init; }

        public required bool IsDeletingInput { get; set; }

        public required string? OutputDirectoryPath { get; set; }
    }

    public sealed record Result
    {
        public required string FullPath { get; init; }
    }
}