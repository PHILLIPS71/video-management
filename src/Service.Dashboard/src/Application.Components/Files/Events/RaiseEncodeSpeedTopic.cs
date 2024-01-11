using Giantnodes.Service.Dashboard.Application.Contracts.Files.Events;
using Giantnodes.Service.Dashboard.Domain.Aggregates.Entries.Files.Repositories;
using HotChocolate.Subscriptions;
using MassTransit;

namespace Giantnodes.Service.Dashboard.Application.Components.Files.Events;

public class RaiseEncodeSpeedTopic : IConsumer<FileEncodeSpeedChangedEvent>
{
    private readonly IFileSystemFileRepository _repository;
    private readonly ITopicEventSender _sender;

    public RaiseEncodeSpeedTopic(IFileSystemFileRepository repository, ITopicEventSender sender)
    {
        _repository = repository;
        _sender = sender;
    }

    public async Task Consume(ConsumeContext<FileEncodeSpeedChangedEvent> context)
    {
        var file = await _repository.SingleAsync(x => x.Id == context.Message.FileId);
        var encode = file.Encodes.First(x => x.Id == context.Message.EncodeId);

        await _sender.SendAsync(nameof(FileEncodeSpeedChangedEvent), encode, context.CancellationToken);
    }
}