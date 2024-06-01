using FluentValidation;
using Giantnodes.Infrastructure.Messages;

namespace Giantnodes.Service.Orchestrator.Application.Contracts.Libraries.Commands;

public sealed class LibraryUpdate
{
    public sealed record Command : Message
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
            RuleFor(p => p.Id)
                .NotEmpty();

            RuleFor(p => p.Name)
                .NotEmpty();

            RuleFor(p => p.Slug)
                .Must(p => p.IsSlug()).WithMessage(p => $"The format of slug '{p.Slug}' is invalid.")
                .NotEmpty();
        }
    }

    public sealed record Result
    {
        public required Guid Id { get; init; }
    }
}