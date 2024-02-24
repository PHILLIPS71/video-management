using FluentValidation;
using Giantnodes.Infrastructure.Messages;

namespace Giantnodes.Service.Encoder.Application.Contracts.Encoding.Jobs;

public sealed class EncodeFile
{
    public sealed record Job : Message
    {
        public required string InputFilePath { get; init; }

        public required string OutputFilePath { get; init; }
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