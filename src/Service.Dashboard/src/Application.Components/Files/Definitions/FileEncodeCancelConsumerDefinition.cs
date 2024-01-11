using Giantnodes.Service.Dashboard.Application.Components.Files.Commands;
using Giantnodes.Service.Dashboard.Persistence.DbContexts;
using MassTransit;

namespace Giantnodes.Service.Dashboard.Application.Components.Files.Definitions;

public class FileEncodeCancelConsumerDefinition : ConsumerDefinition<FileEncodeCancelConsumer>
{
    protected override void ConfigureConsumer(
        IReceiveEndpointConfigurator endpointConfigurator,
        IConsumerConfigurator<FileEncodeCancelConsumer> consumerConfigurator,
        IRegistrationContext context)
    {
        endpointConfigurator.UseEntityFrameworkOutbox<ApplicationDbContext>(context);
    }
}