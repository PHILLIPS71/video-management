using Giantnodes.Infrastructure.Messages;

namespace Giantnodes.Service.Encoder.Application.Contracts.Encoding.Commands;

public sealed class TransferFile
{
    public sealed record Job : Message
    {
        public required string InputFilePath { get; init; }

        public required string OutputFilePath { get; set; }
    }
}