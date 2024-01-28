using Giantnodes.Service.Dashboard.Application.Components.Libraries.Events;
using MassTransit;

namespace Giantnodes.Service.Dashboard.Application.Components.Libraries.Definitions;

public class OnFileSystemChangedScanDefinition : ConsumerDefinition<OnFileSystemChangedScan>
{
    protected override void ConfigureConsumer(
        IReceiveEndpointConfigurator endpointConfigurator,
        IConsumerConfigurator<OnFileSystemChangedScan> consumerConfigurator,
        IRegistrationContext context)
    {
        consumerConfigurator.Options<BatchOptions>(
            options => options
                .SetMessageLimit(int.MaxValue)
                .SetTimeLimit(TimeSpan.FromSeconds(8))
        );
    }
}