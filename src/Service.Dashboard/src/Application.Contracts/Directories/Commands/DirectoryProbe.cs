using FluentValidation;

namespace Giantnodes.Service.Dashboard.Application.Contracts.Directories.Commands;

public sealed class DirectoryProbe
{
    public sealed record Command
    {
        public required Guid DirectoryId { get; init; }
    }

    public sealed class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(p => p.DirectoryId)
                .NotEmpty();
        }
    }

    public sealed record Result
    {
        public required string FullPath { get; init; }
    }
}