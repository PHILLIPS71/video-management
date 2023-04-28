using Giantnodes.Infrastructure.Extensions;
using Giantnodes.Infrastructure.MassTransit;

namespace MassTransit
{
    public static class ConsumeContextExtensions
    {
        public static async Task RejectAsync<TContract, TCode>(this ConsumeContext context, TCode code, string? reason = null)
            where TContract : class, IRejected<TCode>
            where TCode : Enum
        {
            if (context.IsResponseAccepted<TContract>())
            {
                await context.RespondAsync<TContract>(new
                {
                    context.ConversationId,
                    TimeStamp = DateTime.UtcNow,
                    ErrorCode = code,
                    Reason = reason ?? code.GetStringValue()
                });
            }
        }
    }
}