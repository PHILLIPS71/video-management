﻿using Giantnodes.Service.Dashboard.Application.Components.Files.Commands;
using Giantnodes.Service.Dashboard.Persistence.DbContexts;
using MassTransit;

namespace Giantnodes.Service.Dashboard.Application.Components.Files.Definitions;

public class FileTranscodeSubmitConsumerDefinition : ConsumerDefinition<FileTranscodeSubmitConsumer>
{
    protected override void ConfigureConsumer(
        IReceiveEndpointConfigurator endpointConfigurator,
        IConsumerConfigurator<FileTranscodeSubmitConsumer> consumerConfigurator,
        IRegistrationContext context)
    {
        endpointConfigurator.UseEntityFrameworkOutbox<ApplicationDbContext>(context);
    }
}