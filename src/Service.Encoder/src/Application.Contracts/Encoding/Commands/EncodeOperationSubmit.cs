using Giantnodes.Infrastructure.Messages;

namespace Giantnodes.Service.Encoder.Application.Contracts.Encoding.Commands;

public sealed class EncodeOperationSubmit
{
    public sealed record Command : Message
    {
        public required string InputFilePath { get; init; }

        public required string OutputFilePath { get; init; }

        public required string Codec { get; init; }

        public required string Preset { get; init; }

        public string? Tune { get; init; }

        public int? Quality { get; set; }

        public bool UseHardwareAcceleration { get; init; }
    }
}