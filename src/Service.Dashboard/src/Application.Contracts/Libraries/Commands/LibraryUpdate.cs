using FluentValidation;

namespace Giantnodes.Service.Dashboard.Application.Contracts.Libraries.Commands;

public sealed class LibraryUpdate
{
    public sealed record Command
    {
        public required Guid Id { get; init; }

        public required string Name { get; init; }

        public required string Slug { get; init; }

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
        }
    }

    public sealed record Result
    {
        public required Guid Id { get; init; }
    }
}