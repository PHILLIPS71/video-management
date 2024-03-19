using Giantnodes.Service.Dashboard.Persistence.DbContexts;
using Giantnodes.Service.Dashboard.Persistence.Sagas;
using MassTransit;

namespace Giantnodes.Service.Dashboard.Application.Components.Encodes.Sagas;

public class EncodeStateMachineDefinition : SagaDefinition<EncodeSagaState>
{
    protected override void ConfigureSaga(
        IReceiveEndpointConfigurator endpointConfigurator,
        ISagaConfigurator<EncodeSagaState> sagaConfigurator,
        IRegistrationContext context)
    {
        endpointConfigurator.ConcurrentMessageLimit = 3;

        endpointConfigurator.UseMessageRetry(r => r.Interval(3, TimeSpan.FromSeconds(3)));
    }
}