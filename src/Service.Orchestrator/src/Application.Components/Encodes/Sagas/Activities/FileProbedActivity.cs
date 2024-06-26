using Giantnodes.Infrastructure.Uow.Services;
using Giantnodes.Service.Orchestrator.Domain.Aggregates.Encodes.Entities;
using Giantnodes.Service.Orchestrator.Domain.Aggregates.Encodes.Repositories;
using Giantnodes.Service.Orchestrator.Domain.Aggregates.Entries.Files.Values;
using Giantnodes.Service.Orchestrator.Domain.Values;
using Giantnodes.Service.Orchestrator.Persistence.Sagas;
using Giantnodes.Service.Encoder.Application.Contracts.Probing.Events;
using MassTransit;

namespace Giantnodes.Service.Orchestrator.Application.Components.Encodes.Sagas.Activities;

public class FileProbedActivity : IStateMachineActivity<EncodeSagaState, FileProbedEvent>
{
    private readonly IUnitOfWorkService _uow;
    private readonly IEncodeRepository _repository;

    public FileProbedActivity(IUnitOfWorkService uow, IEncodeRepository repository)
    {
        _uow = uow;
        _repository = repository;
    }

    public void Probe(ProbeContext context)
    {
        context.CreateScope(KebabCaseEndpointNameFormatter.Instance.Message<EncodeOperationCompletedActivity>());
    }

    public void Accept(StateMachineVisitor visitor)
    {
        visitor.Visit(this);
    }

    public async Task Execute(
        BehaviorContext<EncodeSagaState, FileProbedEvent> context,
        IBehavior<EncodeSagaState, FileProbedEvent> next)
    {
        using (var uow = await _uow.BeginAsync(context.CancellationToken))
        {
            var encode = await _repository.SingleAsync(x => x.Id == context.Saga.EncodeId);

            var videos = context.Message.VideoStreams
                .Select(x => new VideoStream(x.Index, x.Codec, x.Duration, new VideoQuality(x.Width, x.Height, x.AspectRatio), x.Framerate, x.Bitrate, x.PixelFormat))
                .ToArray();

            var audio = context.Message.AudioStreams
                .Select(x => new AudioStream(x.Index, x.Codec, x.Title, x.Language, x.Duration, x.Bitrate, x.SampleRate, x.Channels))
                .ToArray();

            var subtitles = context.Message.AudioStreams
                .Select(x => new SubtitleStream(x.Index, x.Codec, x.Title, x.Language))
                .ToArray();

            var streams = new List<Domain.Values.FileStream>();
            streams.AddRange(videos);
            streams.AddRange(audio);
            streams.AddRange(subtitles);

            var snapshot = new EncodeSnapshot(encode, context.Message.Size, context.Message.RaisedAt, streams.ToArray());
            encode.AddSnapshot(snapshot);

            await uow.CommitAsync(context.CancellationToken);
        }

        await next.Execute(context);
    }

    public Task Faulted<TException>(
        BehaviorExceptionContext<EncodeSagaState, FileProbedEvent, TException> context,
        IBehavior<EncodeSagaState, FileProbedEvent> next)
        where TException : Exception
    {
        return next.Faulted(context);
    }
}