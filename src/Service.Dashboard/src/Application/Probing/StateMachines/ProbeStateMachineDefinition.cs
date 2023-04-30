using Giantnodes.Service.Dashboard.Persistence.Sagas;
using MassTransit;

namespace Giantnodes.Service.Dashboard.Application.Probing.StateMachines;

public class ProbeStateMachineDefinition : SagaDefinition<ProbeSaga>
{
    public ProbeStateMachineDefinition()
    {
        ConcurrentMessageLimit = 8;
    }

    protected override void ConfigureSaga(IReceiveEndpointConfigurator endpoint, ISagaConfigurator<ProbeSaga> saga)
    {
        saga.UseInMemoryOutbox();
        saga.UseMessageRetry(r => r.Interval(5, TimeSpan.FromMilliseconds(500)));
    }
}