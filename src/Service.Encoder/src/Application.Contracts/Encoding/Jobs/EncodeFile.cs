using FluentValidation;
using Giantnodes.Infrastructure.Messages;

namespace Giantnodes.Service.Encoder.Application.Contracts.Encoding.Jobs;

public sealed class EncodeFile
{
    public sealed record Job : Message
    {
        public required string InputFilePath { get; init; }

        public required string OutputFilePath { get; init; }

        public required string Codec { get; init; }

        public required string Preset { get; init; }

        public string? Tune { get; init; }

        public int? Quality { get; init; }

        public bool UseHardwareAcceleration { get; init; }
    }

    public sealed class Validator : AbstractValidator<Job>
    {
        public Validator()
        {
            RuleFor(p => p.InputFilePath)
                .NotEmpty();

            RuleFor(p => p.OutputFilePath)
                .NotEmpty();
        }
    }
}