using FluentValidation;
using Giantnodes.Infrastructure.Messages;

namespace Giantnodes.Service.Encoder.Application.Contracts.Probing.Jobs;

public sealed class ProbeFileSystem
{
    public sealed record Job : Message
    {
        public required string FilePath { get; init; }
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