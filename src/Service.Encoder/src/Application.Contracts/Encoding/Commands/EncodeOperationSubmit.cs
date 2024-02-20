using Giantnodes.Infrastructure.Messages;

namespace Giantnodes.Service.Encoder.Application.Contracts.Encoding.Commands;

public sealed class EncodeOperationSubmit
{
    public sealed record Command : Message
    {
        public required string FilePath { get; init; }
    }
}