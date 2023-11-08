using FluentValidation;
using Giantnodes.Infrastructure.Faults;

namespace Giantnodes.Service.Dashboard.Application.Contracts.Directories;

public sealed class DirectoryProbe
{
    public sealed record Command
    {
        public required string FullPath { get; init; }
    }

    public sealed class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(p => p.FullPath)
                .NotEmpty();
        }
    }

    public sealed class Fault : FaultKind
    {
        public static readonly FaultKind DirectoryNotFound =
            new(1, FaultType.InvalidRequest, "directory_not_found", "the directory cannot be found.");

        private Fault(int id, FaultType type, string code, string message)
            : base(id, type, code, message)
        {
        }
    }

    public sealed record Result
    {
        public required string FullPath { get; init; }
    }
}