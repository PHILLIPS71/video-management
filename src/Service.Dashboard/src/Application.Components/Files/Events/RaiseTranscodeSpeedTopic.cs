using Giantnodes.Service.Dashboard.Application.Contracts.Files.Events;
using Giantnodes.Service.Dashboard.Domain.Aggregates.Entries.Files.Repositories;
using HotChocolate.Subscriptions;
using MassTransit;

namespace Giantnodes.Service.Dashboard.Application.Components.Files.Events;

public class RaiseTranscodeSpeedTopic : IConsumer<FileTranscodeSpeedChangedEvent>
{
    private readonly IFileSystemFileRepository _repository;
    private readonly ITopicEventSender _sender;

    public RaiseTranscodeSpeedTopic(IFileSystemFileRepository repository, ITopicEventSender sender)
    {
        _repository = repository;
        _sender = sender;
    }

    public async Task Consume(ConsumeContext<FileTranscodeSpeedChangedEvent> context)
    {
        var file = await _repository.SingleAsync(x => x.Id == context.Message.FileId);
        var transcode = file.Transcodes.First(x => x.Id == context.Message.TranscodeId);

        await _sender.SendAsync(nameof(FileTranscodeSpeedChangedEvent), transcode, context.CancellationToken);
    }
}