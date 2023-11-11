using MassTransit;

namespace Giantnodes.Service.Encoder.Application.Components.Encoding.Jobs;

public class TranscodeConsumerDefinition : ConsumerDefinition<TranscodeConsumer>
{
    protected override void ConfigureConsumer(
        IReceiveEndpointConfigurator endpointConfigurator,
        IConsumerConfigurator<TranscodeConsumer> consumerConfigurator,
        IRegistrationContext context)
    {
        consumerConfigurator
            .Options<JobOptions<TranscodeConsumer>>(options =>
                options
                    .SetRetry(r => r.Interval(3, TimeSpan.FromSeconds(30)))
                    .SetJobTimeout(TimeSpan.FromDays(7))
                    .SetConcurrentJobLimit(3)
            );
    }
}