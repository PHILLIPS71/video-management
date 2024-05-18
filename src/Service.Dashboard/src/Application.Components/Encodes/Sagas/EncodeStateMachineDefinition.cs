using Giantnodes.Service.Dashboard.Application.Contracts.Encodes.Events;
using Giantnodes.Service.Dashboard.Persistence.Sagas;
using Giantnodes.Service.Encoder.Application.Contracts.Encoding.Events;
using Giantnodes.Service.Encoder.Application.Contracts.Probing.Events;
using MassTransit;

namespace Giantnodes.Service.Dashboard.Application.Components.Encodes.Sagas;

public class EncodeStateMachineDefinition : SagaDefinition<EncodeSagaState>
{
    private const int ConcurrencyLimit = 25;

    protected override void ConfigureSaga(
        IReceiveEndpointConfigurator endpointConfigurator,
        ISagaConfigurator<EncodeSagaState> sagaConfigurator,
        IRegistrationContext context)
    {
        endpointConfigurator.UseMessageRetry(r => r.Interval(3, TimeSpan.FromSeconds(3)));

        var partition = sagaConfigurator.CreatePartitioner(ConcurrencyLimit);
        endpointConfigurator.UsePartitioner<EncodeCreatedEvent>(partition, p => p.Message.CorrelationId);
        endpointConfigurator.UsePartitioner<FileProbedEvent>(partition, p => p.Message.JobId);
        endpointConfigurator.UsePartitioner<EncodeOperationStartedEvent>(partition, p => p.Message.CorrelationId);
        endpointConfigurator.UsePartitioner<EncodeOperationEncodeBuiltEvent>(partition, p => p.Message.CorrelationId);
        endpointConfigurator.UsePartitioner<EncodeOperationEncodeProgressedEvent>(partition, p => p.Message.CorrelationId);
        endpointConfigurator.UsePartitioner<EncodeOperationOutputtedEvent>(partition, p => p.Message.CorrelationId);
        endpointConfigurator.UsePartitioner<EncodeOperationCompletedEvent>(partition, p => p.Message.CorrelationId);
        endpointConfigurator.UsePartitioner<EncodeCancelledEvent>(partition, p => p.Message.CorrelationId);
        endpointConfigurator.UsePartitioner<EncodeOperationFailedEvent>(partition, p => p.Message.CorrelationId);
    }
}