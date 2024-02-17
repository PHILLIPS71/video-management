using FluentValidation;

namespace Giantnodes.Service.Dashboard.Application.Contracts.Libraries.Commands;

public sealed class LibraryCreate
{
    public sealed record Command
    {
        public required string Name { get; init; }

        public required string Slug { get; init; }

        public required string DirectoryPath { get; init; }

        public bool IsWatched { get; init; }
    }

    public sealed class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(p => p.Name)
                .NotEmpty();

            RuleFor(p => p.Slug)
                .Must(p => p.IsSlug()).WithMessage(p => $"The format of slug '{p.Slug}' is invalid.")
                .NotEmpty();

            RuleFor(p => p.DirectoryPath)
                .NotEmpty();
        }
    }

    public sealed record Result
    {
        public required Guid Id { get; init; }
    }
}