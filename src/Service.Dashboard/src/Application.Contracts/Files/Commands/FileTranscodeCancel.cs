using FluentValidation;
using Giantnodes.Infrastructure.Faults;

namespace Giantnodes.Service.Dashboard.Application.Contracts.Files.Commands;

public sealed class FileTranscodeCancel
{
    public sealed record Command
    {
        public required Guid FileId { get; init; }

        public required Guid TranscodeId { get; init; }
    }

    public sealed class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(p => p.TranscodeId)
                .NotEmpty();
        }
    }

    public sealed class Fault : FaultKind
    {
        public static readonly FaultKind FileNotFound =
            new(1, FaultType.InvalidRequest, "not_found", "the file cannot be found in the library.");

        private Fault(int id, FaultType type, string code, string message)
            : base(id, type, code, message)
        {
        }
    }

    public sealed record Result
    {
        public required Guid TranscodeId { get; init; }
    }
}