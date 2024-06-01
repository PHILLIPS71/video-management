using FluentValidation;
using Giantnodes.Infrastructure.Messages;

namespace Giantnodes.Service.Orchestrator.Application.Contracts.Directories.Commands;

public sealed class DirectoryProbe
{
    public sealed record Command : Message
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
        public required string FilePath { get; init; }
    }
}