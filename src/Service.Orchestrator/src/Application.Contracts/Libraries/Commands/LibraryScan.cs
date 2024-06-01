using FluentValidation;
using Giantnodes.Infrastructure.Messages;

namespace Giantnodes.Service.Orchestrator.Application.Contracts.Libraries.Commands;

public sealed class LibraryScan
{
    public sealed record Command : Message
    {
        public required Guid LibraryId { get; init; }
    }

    public sealed class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(p => p.LibraryId)
                .NotEmpty();
        }
    }

    public sealed record Result
    {
        public required Guid LibraryId { get; init; }
    }
}