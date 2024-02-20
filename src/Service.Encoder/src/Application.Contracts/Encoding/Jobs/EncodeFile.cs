using FluentValidation;
using Giantnodes.Infrastructure.Messages;

namespace Giantnodes.Service.Encoder.Application.Contracts.Encoding.Jobs;

public sealed class EncodeFile
{
    public sealed record Job : Message
    {
        public required string FilePath { get; init; }

        public string? FileContainer { get; init; }
    }

    public sealed class Validator : AbstractValidator<Job>
    {
        public Validator()
        {
            RuleFor(p => p.FilePath)
                .NotEmpty();
        }
    }
}