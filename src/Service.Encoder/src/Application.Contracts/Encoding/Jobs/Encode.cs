using FluentValidation;
using Giantnodes.Infrastructure.Faults;

namespace Giantnodes.Service.Encoder.Application.Contracts.Encoding.Jobs;

public sealed class Encode
{
    public sealed record Job
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