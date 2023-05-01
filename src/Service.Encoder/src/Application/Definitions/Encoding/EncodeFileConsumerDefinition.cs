using Giantnodes.Service.Encoder.Application.Components.Encoding.Jobs;
using Giantnodes.Service.Encoder.Application.Contracts.Encoding.Jobs;
using MassTransit;

namespace Giantnodes.Service.Encoder.Application.Definitions.Encoding;

public class EncodeFileConsumerDefinition : ConsumerDefinition<EncodeFileConsumer>
{
    protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpoint, IConsumerConfigurator<EncodeFileConsumer> consumer)
    {
        consumer
            .Options<JobOptions<EncodeFile>>(options =>
                options
                    .SetRetry(r => r.Interval(3, TimeSpan.FromSeconds(3)))
                    .SetJobTimeout(TimeSpan.FromDays(3))
            );
    }
}