using FluentValidation;

namespace Giantnodes.Service.Dashboard.Application.Contracts.Files.Commands;

public sealed class FileEncodeSubmit
{
    public sealed record Command
    {
        public required Guid[] Entries { get; init; }
    }

    public sealed class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(p => p.Entries)
                .NotEmpty();
        }
    }

    public sealed record Result
    {
        public required Guid[] Encodes { get; init; }
    }
}