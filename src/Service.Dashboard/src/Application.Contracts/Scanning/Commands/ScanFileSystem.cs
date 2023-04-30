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
        public required Guid ConversationId { get; init; }
       
        public required DateTime TimeStamp { get; init; }
       
        public required Rejection ErrorCode { get; init; }
        
        public required string Reason { get; init; }
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