using Giantnodes.Service.Dashboard.Persistence.Sagas;
using Giantnodes.Service.Encoder.Application.Contracts.Encoding.Events;
using MassTransit;

namespace Giantnodes.Service.Dashboard.Application.Components.Encodes.Sagas;

public class EncodeStateMachineDefinition : SagaDefinition<EncodeSagaState>
{
    protected override void ConfigureSaga(
        IReceiveEndpointConfigurator endpointConfigurator,
        ISagaConfigurator<EncodeSagaState> sagaConfigurator,
        IRegistrationContext context)
    {
        endpointConfigurator.UseMessageRetry(r => r.Interval(3, TimeSpan.FromSeconds(3)));

        var partition = sagaConfigurator.CreatePartitioner(1);
        endpointConfigurator.UsePartitioner<EncodeOperationEncodeHeartbeatEvent>(partition, p => p.Message.JobId);
        endpointConfigurator.UsePartitioner<EncodeOperationEncodeProgressedEvent>(partition, p => p.Message.JobId);
        endpointConfigurator.UsePartitioner<EncodeOperationOutputtedEvent>(partition, p => p.Message.JobId);
    }
}