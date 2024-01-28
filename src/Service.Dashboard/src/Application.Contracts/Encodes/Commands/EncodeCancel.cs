using FluentValidation;

namespace Giantnodes.Service.Dashboard.Application.Contracts.Encodes.Commands;

public sealed class EncodeCancel
{
    public sealed record Command
    {
        public required Guid EncodeId { get; init; }
    }

    public sealed class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(p => p.EncodeId)
                .NotEmpty();
        }
    }

    public sealed record Result
    {
        public required Guid EncodeId { get; init; }
    }
}