using Giantnodes.Service.Encoder.Persistence.DbContexts;
using Giantnodes.Service.Encoder.Persistence.Sagas;
using MassTransit;

namespace Giantnodes.Service.Encoder.Application.Components.Encoding.Definitions;

public class EncodeJobStateMachineDefinition : SagaDefinition<EncodeJobSaga>
{
    protected override void ConfigureSaga(
        IReceiveEndpointConfigurator endpointConfigurator,
        ISagaConfigurator<EncodeJobSaga> sagaConfigurator,
        IRegistrationContext context)
    {
        endpointConfigurator.ConcurrentMessageLimit = 3;

        endpointConfigurator.UseMessageRetry(r => r.Interval(3, TimeSpan.FromSeconds(3)));
        endpointConfigurator.UseEntityFrameworkOutbox<ApplicationDbContext>(context);
    }
}