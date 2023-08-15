using Giantnodes.Infrastructure.Faults;
using Giantnodes.Infrastructure.Faults.Types;

namespace MassTransit;

public static class ConsumeContextExtensions
{
    public static async Task RejectAsync(this ConsumeContext context, FaultKind kind, string? property = null)
    {
        if (!context.IsResponseAccepted<DomainFault>())
            throw new InvalidOperationException(kind.Message);

        await context.RespondAsync(new DomainFault
        {
            Type = kind.Type,
            RequestId = context.RequestId,
            TimeStamp = InVar.Timestamp,
            Code = kind.Code,
            Message = kind.Message,
            Property = property
        });
    }
}