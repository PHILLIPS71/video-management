﻿using Giantnodes.Infrastructure.Messages;

namespace Giantnodes.Service.Encoder.Application.Contracts.Encoding.Commands;

public sealed class EncodeOperationSubmit
{
    public sealed record Command : Message
    {
        public required string InputFilePath { get; init; }

        public required string OutputDirectoryPath { get; init; }

        public string? Container { get; init; }
    }
}