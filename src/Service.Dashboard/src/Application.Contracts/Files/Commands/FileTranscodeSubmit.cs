using FluentValidation;

namespace Giantnodes.Service.Dashboard.Application.Contracts.Files.Commands;

public sealed class FileTranscodeSubmit
{
    public sealed record Command
    {
        public required Guid FileId { get; init; }
    }

    public sealed class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(p => p.FileId)
                .NotEmpty();
        }
    }

    public sealed record Result
    {
        public required Guid TranscodeId { get; init; }
    }
}