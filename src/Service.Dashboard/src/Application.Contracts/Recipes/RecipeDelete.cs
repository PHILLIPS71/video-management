using FluentValidation;
using Giantnodes.Infrastructure.Messages;

namespace Giantnodes.Service.Dashboard.Application.Contracts.Recipes;

public sealed class RecipeDelete
{
    public sealed record Command : Message
    {
        public required Guid Id { get; init; }
    }

    public sealed class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(p => p.Id)
                .NotEmpty();
        }
    }

    public sealed record Result
    {
        public required Guid Id { get; init; }
    }
}