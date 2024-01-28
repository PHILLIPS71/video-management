using Giantnodes.Service.Dashboard.Application.Components.Files.Sagas.Activities;
using Giantnodes.Service.Dashboard.Application.Contracts.Encodes.Events;
using Giantnodes.Service.Dashboard.Persistence.Sagas;
using Giantnodes.Service.Encoder.Application.Contracts.Encoding.Commands;
using Giantnodes.Service.Encoder.Application.Contracts.Encoding.Events;
using Giantnodes.Service.Encoder.Application.Contracts.Probing.Events;
using Giantnodes.Service.Encoder.Application.Contracts.Probing.Jobs;
using MassTransit;
using MassTransit.Contracts.JobService;
using EncodeCancelledEvent = Giantnodes.Service.Dashboard.Application.Contracts.Encodes.Events.EncodeCancelledEvent;

namespace Giantnodes.Service.Dashboard.Application.Components.Files.Sagas;

public class EncodeStateMachine : MassTransitStateMachine<EncodeSagaState>
{
    public EncodeStateMachine()
    {
        InstanceState(x => x.CurrentState);

        Event(() => Submitted, e => e.CorrelateById(context => context.Message.EncodeId));
        Event(() => Cancelled, e => e.CorrelateById(context => context.Message.EncodeId));
        Event(() => FileProbed, e => e.CorrelateById(context => context.Message.JobId));
        Event(() => EncodeStarted);
        Event(() => EncodeCompleted);
        Event(() => Heartbeat);
        Event(() => Progressed);
        Event(() => Failed);

        Request(() => EncodeRequest);

        Initially(
            When(Submitted)
                .Then(context =>
                {
                    context.Saga.InputFullPath = context.Message.FullPath;
                    context.Saga.SubmittedAt = DateTime.UtcNow;
                })
                .PublishAsync(context => context.Init<SubmitJob<ProbeFileSystem.Job>>(new
                {
                    JobId = context.Saga.CorrelationId,
                    Job = new ProbeFileSystem.Job
                    {
                        FullPath = context.Message.FullPath
                    }
                }))
                .TransitionTo(Probing));

        During(Probing,
            When(FileProbed)
                .Activity(context => context.OfType<FileProbedActivity>())
                .Request(EncodeRequest, context => new EncodeSubmit.Command
                {
                    CorrelationId = context.Saga.CorrelationId,
                    InputPath = context.Saga.InputFullPath,
                    OutputDirectoryPath = context.Message.FullPath,
                    IsDeletingInput = false
                })
                .TransitionTo(EncodeRequest?.Pending));

        During(EncodeRequest?.Pending,
            When(EncodeRequest?.Completed)
                .TransitionTo(Queued),
            When(EncodeRequest?.TimeoutExpired)
                .Activity(context => context.OfInstanceType<EncodeDegradeActivity>()),
            When(EncodeRequest?.Faulted)
                .Activity(context => context.OfInstanceType<EncodeDegradeActivity>())
                .Finalize());

        During(EncodeRequest?.Pending, Queued,
            When(EncodeStarted)
                .Activity(context => context.OfType<EncodeStartedActivity>())
                .TransitionTo(Processing));

        During(Processing,
            When(Heartbeat)
                .Activity(context => context.OfType<EncodeHeartbeatActivity>()),
            When(Progressed)
                .Activity(context => context.OfType<EncodeProgressedActivity>()),
            When(EncodeCompleted)
                .Then(context => context.Saga.OutputFullPath = context.Message.OutputPath)
                .Activity(context => context.OfType<EncodeCompletedActivity>())
                .PublishAsync(context => context.Init<SubmitJob<ProbeFileSystem.Job>>(new
                {
                    JobId = context.Saga.CorrelationId,
                    Job = new ProbeFileSystem.Job
                    {
                        FullPath = context.Message.OutputPath
                    }
                }))
                .TransitionTo(Encoded));

        During(Encoded,
            When(FileProbed)
                .Finalize());

        DuringAny(
            When(Failed)
                .Activity(context => context.OfType<EncodeFailedActivity>())
                .Finalize(),
            When(Cancelled)
                .PublishAsync(context => context.Init<EncodeCancel.Command>(new { context.Saga.CorrelationId }))
                .Finalize());

        SetCompletedWhenFinalized();
    }

    public required State Queued { get; set; }
    public required State Probing { get; set; }
    public required State Processing { get; set; }

    public required State Encoded { get; set; }

    public required Event<EncodeCreatedEvent> Submitted { get; set; }
    public required Event<FileProbedEvent> FileProbed { get; set; }
    public required Event<EncodeStartedEvent> EncodeStarted { get; set; }
    public required Event<EncodeCompletedEvent> EncodeCompleted { get; set; }
    public required Event<EncodeHeartbeatEvent> Heartbeat { get; set; }
    public required Event<EncodeProgressedEvent> Progressed { get; set; }
    public required Event<EncodeCancelledEvent> Cancelled { get; set; }
    public required Event<EncodeFailedEvent> Failed { get; set; }

    public required Request<EncodeSagaState, EncodeSubmit.Command, EncodeSubmit.Result> EncodeRequest { get; set; }
}