using Giantnodes.Service.Dashboard.Application.Components.Files.Sagas.Activities;
using Giantnodes.Service.Dashboard.Application.Contracts.Files.Events;
using Giantnodes.Service.Dashboard.Persistence.Sagas;
using Giantnodes.Service.Encoder.Application.Contracts.Encoding.Commands;
using Giantnodes.Service.Encoder.Application.Contracts.Encoding.Events;
using MassTransit;

namespace Giantnodes.Service.Dashboard.Application.Components.Files.Sagas;

public class EncodeStateMachine : MassTransitStateMachine<EncodeSagaState>
{
    public EncodeStateMachine()
    {
        InstanceState(x => x.CurrentState);

        Event(() => Submitted, e => e.CorrelateById(context => context.Message.EncodeId));
        Event(() => Cancelled, e => e.CorrelateById(context => context.Message.EncodeId));
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
                .Activity(context => context.OfType<EncodeCompletedActivity>())
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
    public required State Processing { get; set; }

    public required Event<FileEncodeCreatedEvent> Submitted { get; set; }
    public required Event<EncodeStartedEvent> EncodeStarted { get; set; }
    public required Event<EncodeCompletedEvent> EncodeCompleted { get; set; }
    public required Event<EncodeHeartbeatEvent> Heartbeat { get; set; }
    public required Event<EncodeProgressedEvent> Progressed { get; set; }
    public required Event<FileEncodeCancelledEvent> Cancelled { get; set; }
    public required Event<EncodeFailedEvent> Failed { get; set; }

    public required Request<EncodeSagaState, EncodeSubmit.Command, EncodeSubmit.Result> EncodeRequest { get; set; }
}