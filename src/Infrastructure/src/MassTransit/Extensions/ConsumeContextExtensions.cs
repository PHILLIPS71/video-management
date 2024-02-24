using Giantnodes.Infrastructure.Faults;
using Giantnodes.Infrastructure.Faults.Exceptions;
using Giantnodes.Infrastructure.Faults.Types;

namespace MassTransit;

public static class ConsumeContextExtensions
{
    /// <summary>
    /// Extension method to reject a message by responding with a domain fault asynchronously.
    /// </summary>
    /// <param name="context">The ConsumeContext representing the message context.</param>
    /// <param name="kind">The kind of fault to be rejected.</param>
    /// <param name="property">An optional property associated with the fault.</param>
    /// <returns>A task representing the asynchronous rejection operation.</returns>
    public static async Task RejectAsync(this ConsumeContext context, FaultKind kind, string? property = null)
    {
        var fault = context.ToDomainFault(kind, property);

        if (!context.IsResponseAccepted<DomainFault>())
            throw new DomainException(fault);

        await context.RespondAsync(fault);
    }

    /// <summary>
    /// Extension method to reject a message by responding with a domain fault asynchronously.
    /// </summary>
    /// <param name="context">The JobContext representing the job context.</param>
    /// <param name="kind">The kind of fault to be rejected.</param>
    /// <param name="property">An optional property associated with the fault.</param>
    /// <returns>A task representing the asynchronous rejection operation.</returns>
    public static Task RejectAsync(this JobContext context, FaultKind kind, string? property = null)
    {
        var fault = context.ToDomainFault(kind, property);
        throw new DomainException(fault);
    }

    /// <summary>
    /// Converts a MessageContext to a DomainFault based on the specified fault kind, with an optional property.
    /// </summary>
    /// <param name="context">The MessageContext representing the message context.</param>
    /// <param name="kind">The kind of fault to be converted.</param>
    /// <param name="property">An optional property associated with the fault.</param>
    /// <returns>A DomainFault representing the converted fault.</returns>
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