﻿using Giantnodes.Service.Encoder.Application.Components.Encoding.Jobs;
using Giantnodes.Service.Encoder.Application.Components.Settings;
using Giantnodes.Service.Encoder.Application.Contracts.Encoding.Jobs;
using MassTransit;
using Microsoft.Extensions.Options;

namespace Giantnodes.Service.Encoder.Application.Components.Encoding.Definitions;

public class EncodeFileDefinition : ConsumerDefinition<EncodeFileConsumer>
{
    private readonly IOptions<LimitSettings> _limits;

    public EncodeFileDefinition(IOptions<LimitSettings> limits)
    {
        _limits = limits;
    }

    protected override void ConfigureConsumer(
        IReceiveEndpointConfigurator endpointConfigurator,
        IConsumerConfigurator<EncodeFileConsumer> consumerConfigurator,
        IRegistrationContext context)
    {
        consumerConfigurator
            .Options<JobOptions<EncodeFile.Job>>(options =>
                options
                    .SetRetry(r => r.Interval(3, TimeSpan.FromSeconds(30)))
                    .SetJobTimeout(TimeSpan.FromDays(7))
                    .SetConcurrentJobLimit(_limits.Value.MaxConcurrentEncodes)
            );
    }
}