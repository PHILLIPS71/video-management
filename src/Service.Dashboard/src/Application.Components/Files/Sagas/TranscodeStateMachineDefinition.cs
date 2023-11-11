using Giantnodes.Service.Dashboard.Persistence.Sagas;
using MassTransit;

namespace Giantnodes.Service.Dashboard.Application.Components.Files.Sagas;

public class TranscodeStateMachineDefinition : SagaDefinition<TranscodeSagaState>
{
    protected override void ConfigureSaga(
        IReceiveEndpointConfigurator endpointConfigurator,
        ISagaConfigurator<TranscodeSagaState> sagaConfigurator,
        IRegistrationContext context)
    {
        endpointConfigurator.ConcurrentMessageLimit = 3;

        endpointConfigurator.UseInMemoryOutbox(context);
        endpointConfigurator.UseMessageRetry(r => r.Interval(3, TimeSpan.FromSeconds(3)));
    }
}