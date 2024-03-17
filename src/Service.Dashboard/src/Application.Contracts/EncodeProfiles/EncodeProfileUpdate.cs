using FluentValidation;
using Giantnodes.Infrastructure.Messages;

namespace Giantnodes.Service.Dashboard.Application.Contracts.EncodeProfiles;

public sealed class EncodeProfileUpdate
{
    public sealed record Command : Message
    {
        public required Guid Id { get; init; }

        public required string Name { get; init; }

        public int? Container { get; init; }

        public required int Codec { get; init; }

        public required int Preset { get; init; }

        public int? Tune { get; init; }

        public int? Quality { get; init; }
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