using System.ComponentModel;
using FluentValidation;
using Giantnodes.Infrastructure.MassTransit;

namespace Giantnodes.Service.Dashboard.Application.Contracts.Libraries.Commands;

public static class CreateLibrary
{
    public sealed record Command
    {
        public required string Name { get; init; }

        public required string Slug { get; init; } 

        public required string FullPath { get; init; }
    }
    
    public sealed record Result
    {
        public required Guid Id { get; init; }
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
        [Description("A library with this name already exists")]
        Duplicate,
        
        [Description("The directory cannot be found")]
        NotFound,
        
        [Description("The directory cannot be read or written to")]
        Unauthorized
    }
    
    public sealed class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(p => p.Name)
                .NotEmpty();
            
            RuleFor(p => p.Slug)
                .NotEmpty();
            
            RuleFor(p => p.FullPath)
                .NotEmpty();
        }
    }
}