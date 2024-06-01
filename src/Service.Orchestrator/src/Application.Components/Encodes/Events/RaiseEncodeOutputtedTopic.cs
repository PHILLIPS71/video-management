using Giantnodes.Service.Orchestrator.Application.Contracts.Encodes.Events;
using Giantnodes.Service.Orchestrator.Domain.Aggregates.Encodes.Repositories;
using HotChocolate.Subscriptions;
using MassTransit;

namespace Giantnodes.Service.Orchestrator.Application.Components.Encodes.Events;

public class RaiseEncodeOutputtedTopic : IConsumer<EncodeProgressedEvent>
{
    private readonly IEncodeRepository _repository;
    private readonly ITopicEventSender _sender;

    public RaiseEncodeOutputtedTopic(IEncodeRepository repository, ITopicEventSender sender)
    {
        _repository = repository;
        _sender = sender;
    }

    public async Task Consume(ConsumeContext<EncodeProgressedEvent> context)
    {
        var encode = await _repository.SingleAsync(x => x.Id == context.Message.EncodeId);

        await _sender.SendAsync(nameof(EncodeOutputtedEvent), encode, context.CancellationToken);
    }
}