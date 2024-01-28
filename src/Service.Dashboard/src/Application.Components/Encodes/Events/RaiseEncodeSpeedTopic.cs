using Giantnodes.Service.Dashboard.Application.Contracts.Encodes.Events;
using Giantnodes.Service.Dashboard.Domain.Aggregates.Encodes.Repositories;
using HotChocolate.Subscriptions;
using MassTransit;

namespace Giantnodes.Service.Dashboard.Application.Components.Encodes.Events;

public class RaiseEncodeSpeedTopic : IConsumer<EncodeSpeedChangedEvent>
{
    private readonly IEncodeRepository _repository;
    private readonly ITopicEventSender _sender;

    public RaiseEncodeSpeedTopic(IEncodeRepository repository, ITopicEventSender sender)
    {
        _repository = repository;
        _sender = sender;
    }

    public async Task Consume(ConsumeContext<EncodeSpeedChangedEvent> context)
    {
        var encode = await _repository.SingleAsync(x => x.Id == context.Message.FileId);

        await _sender.SendAsync(nameof(EncodeSpeedChangedEvent), encode, context.CancellationToken);
    }
}