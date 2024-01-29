using FluentValidation;

namespace Giantnodes.Service.Encoder.Application.Contracts.Probing.Jobs;

public sealed class ProbeFileSystem
{
    public sealed record Job
    {
        public required string FullPath { get; init; }
    }

    public sealed class Validator : AbstractValidator<Job>
    {
        public Validator()
        {
            RuleFor(p => p.FullPath)
                .NotEmpty();
        }
    }
}