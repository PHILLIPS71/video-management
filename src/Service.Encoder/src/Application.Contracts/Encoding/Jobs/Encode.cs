using FluentValidation;
using Giantnodes.Infrastructure.Faults;

namespace Giantnodes.Service.Encoder.Application.Contracts.Encoding.Jobs;

public sealed class Encode
{
    public sealed record Job
    {
        public required string FullPath { get; init; }

        public string? Container { get; init; }
    }

    public sealed class Validator : AbstractValidator<Job>
    {
        public Validator()
        {
            RuleFor(p => p.FullPath)
                .NotEmpty();
        }
    }

    public sealed class Fault : FaultKind
    {
        public static readonly FaultKind PathNotFound =
            new(1, FaultType.InvalidRequest, "path_not_found", "the path cannot be found on the file system.");

        private Fault(int id, FaultType type, string code, string message)
            : base(id, type, code, message)
        {
        }
    }
}