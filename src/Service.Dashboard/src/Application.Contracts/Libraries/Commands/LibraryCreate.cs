using FluentValidation;
using Giantnodes.Infrastructure.Faults;

namespace Giantnodes.Service.Dashboard.Application.Contracts.Libraries.Commands;

public sealed class LibraryCreate
{
    public sealed record Command
    {
        public required string Name { get; init; }

        public required string Slug { get; init; }

        public required string FullPath { get; init; }

        public bool IsWatched { get; init; }
    }

    public sealed class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(p => p.Name)
                .NotEmpty();

            RuleFor(p => p.Slug)
                .NotEmpty();

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
        public required Guid Id { get; init; }
    }
}