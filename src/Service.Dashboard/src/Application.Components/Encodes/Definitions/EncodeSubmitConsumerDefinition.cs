using Giantnodes.Service.Dashboard.Application.Components.Encodes.Commands;
using Giantnodes.Service.Dashboard.Persistence.DbContexts;
using MassTransit;

namespace Giantnodes.Service.Dashboard.Application.Components.Encodes.Definitions;

public class EncodeSubmitConsumerDefinition : ConsumerDefinition<EncodeSubmitConsumer>
{
    protected override void ConfigureConsumer(
        IReceiveEndpointConfigurator endpointConfigurator,
        IConsumerConfigurator<EncodeSubmitConsumer> consumerConfigurator,
        IRegistrationContext context)
    {
        endpointConfigurator.UseEntityFrameworkOutbox<ApplicationDbContext>(context);
    }
}