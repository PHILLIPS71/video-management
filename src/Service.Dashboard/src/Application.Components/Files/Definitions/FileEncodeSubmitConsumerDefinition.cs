using Giantnodes.Service.Dashboard.Application.Components.Files.Commands;
using Giantnodes.Service.Dashboard.Persistence.DbContexts;
using MassTransit;

namespace Giantnodes.Service.Dashboard.Application.Components.Files.Definitions;

public class FileEncodeSubmitConsumerDefinition : ConsumerDefinition<FileEncodeSubmitConsumer>
{
    protected override void ConfigureConsumer(
        IReceiveEndpointConfigurator endpointConfigurator,
        IConsumerConfigurator<FileEncodeSubmitConsumer> consumerConfigurator,
        IRegistrationContext context)
    {
        endpointConfigurator.UseEntityFrameworkOutbox<ApplicationDbContext>(context);
    }
}