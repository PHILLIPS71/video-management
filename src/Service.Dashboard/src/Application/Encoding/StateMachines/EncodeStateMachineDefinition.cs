using Giantnodes.Service.Dashboard.Persistence.Sagas;
using MassTransit;

namespace Giantnodes.Service.Dashboard.Application.Encoding.StateMachines;

public class EncodeStateMachineDefinition : SagaDefinition<EncodeSaga>
{
    public EncodeStateMachineDefinition()
    {
        ConcurrentMessageLimit = 8;
    }

    protected override void ConfigureSaga(IReceiveEndpointConfigurator endpoint, ISagaConfigurator<EncodeSaga> saga)
    {
        saga.UseInMemoryOutbox();
        saga.UseMessageRetry(r => r.Interval(5, TimeSpan.FromMilliseconds(500)));
    }
}