using System.ComponentModel;
using FluentValidation;
using Giantnodes.Infrastructure.MassTransit;

namespace Giantnodes.Service.Dashboard.Application.Contracts.Scanning.Commands;

public static class ScanFileSystem
{
    public sealed record Command
    {
        public required string FullPath { get; init; }
    }
    
    public sealed record Result
    {
        public required string FullPath { get; init; }
    }
    
    public sealed record Rejected : IRejected<Rejection>
    {
        public Guid ConversationId { get; init; }
       
        public DateTime TimeStamp { get; init; }
       
        public Rejection ErrorCode { get; init; }
        
        public string Reason { get; init; } = string.Empty;
    }

    public enum Rejection
    {
        [Description("The library the directory belongs to cannot be found")]
        LibraryNotFound,
        
        [Description("The directory cannot be found")]
        DirectoryNotFound
    }
    
    public sealed class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(p => p.FullPath)
                .NotEmpty();
        }
    }
}