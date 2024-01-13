using FluentValidation;

namespace Giantnodes.Service.Dashboard.Application.Contracts.Libraries.Commands;

public sealed class LibraryScan
{
    public sealed record Command
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