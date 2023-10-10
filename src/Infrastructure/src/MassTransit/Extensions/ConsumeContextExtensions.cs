using Giantnodes.Infrastructure.Faults;
using Giantnodes.Infrastructure.Faults.Exceptions;
using Giantnodes.Infrastructure.Faults.Types;

namespace MassTransit;

public static class ConsumeContextExtensions
{
    public static async Task RejectAsync(this ConsumeContext context, FaultKind kind, string? property = null)
    {
        var fault = context.ToDomainFault(kind, property);

        if (!context.IsResponseAccepted<DomainFault>())
            throw new DomainException(fault);

        await context.RespondAsync(fault);
    }

    public static Task RejectAsync(this JobContext context, FaultKind kind, string? property = null)
    {
        var fault = context.ToDomainFault(kind, property);
        throw new DomainException(fault);
    }

    private static DomainFault ToDomainFault(this MessageContext context, FaultKind kind, string? property = null)
    {
        return new DomainFault
        {
            Type = kind.Type,
            RequestId = context.RequestId,
            TimeStamp = InVar.Timestamp,
            Code = kind.Code,
            Message = kind.Message,
            Property = property
        };
    }
}