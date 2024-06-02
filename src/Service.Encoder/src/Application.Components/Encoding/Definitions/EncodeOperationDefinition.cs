using Giantnodes.Service.Encoder.Persistence.DbContexts;
using Giantnodes.Service.Encoder.Persistence.Sagas;
using MassTransit;

namespace Giantnodes.Service.Encoder.Application.Components.Encoding.Definitions;

public class EncodeOperationDefinition : SagaDefinition<EncodeOperationSagaState>
{
    protected override void ConfigureSaga(
        IReceiveEndpointConfigurator endpointConfigurator,
        ISagaConfigurator<EncodeOperationSagaState> sagaConfigurator,
        IRegistrationContext context)
    {
        endpointConfigurator.UseMessageRetry(r => r.Interval(3, TimeSpan.FromSeconds(3)));
        endpointConfigurator.UseEntityFrameworkOutbox<ApplicationDbContext>(context);
    }
}