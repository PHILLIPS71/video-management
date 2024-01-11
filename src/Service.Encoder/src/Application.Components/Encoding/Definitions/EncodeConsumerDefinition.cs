using Giantnodes.Service.Encoder.Application.Components.Encoding.Jobs;
using Giantnodes.Service.Encoder.Application.Contracts.Encoding.Jobs;
using MassTransit;

namespace Giantnodes.Service.Encoder.Application.Components.Encoding.Definitions;

public class EncodeConsumerDefinition : ConsumerDefinition<EncodeConsumer>
{
    protected override void ConfigureConsumer(
        IReceiveEndpointConfigurator endpointConfigurator,
        IConsumerConfigurator<EncodeConsumer> consumerConfigurator,
        IRegistrationContext context)
    {
        consumerConfigurator
            .Options<JobOptions<Encode.Job>>(options =>
                options
                    .SetRetry(r => r.Interval(3, TimeSpan.FromSeconds(30)))
                    .SetJobTimeout(TimeSpan.FromDays(7))
                    .SetConcurrentJobLimit(3)
            );
    }
}