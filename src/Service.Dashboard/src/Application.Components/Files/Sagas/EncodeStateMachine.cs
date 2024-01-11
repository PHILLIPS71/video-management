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

        Event(() => EncodeCreated, e => e.CorrelateById(context => context.Message.EncodeId));
        Event(() => EncodeStarted);
        Event(() => EncodeCompleted);
        Event(() => EncodeFailed);
        Event(() => EncodeProgressed);
        Event(() => EncodeHeartbeat);

        Request(() => EncodeSubmitRequest);

        Initially(
            When(EncodeCreated)
                .Then(context =>
                {
                    context.Saga.InputFullPath = context.Message.FullPath;
                    context.Saga.SubmittedAt = DateTime.UtcNow;
                })
                .Request(EncodeSubmitRequest, context => new EncodeSubmit.Command
                {
                    CorrelationId = context.Saga.CorrelationId,
                    InputPath = context.Saga.InputFullPath,
                    OutputDirectoryPath = context.Message.FullPath,
                    IsDeletingInput = false
                })
                .TransitionTo(EncodeSubmitRequest?.Pending));

        During(EncodeSubmitRequest?.Pending,
            When(EncodeSubmitRequest?.Completed)
                .TransitionTo(Queued),
            When(EncodeSubmitRequest?.TimeoutExpired)
                .Activity(context => context.OfInstanceType<EncodeDegradeActivity>())
                .Finalize(),
            When(EncodeSubmitRequest?.Faulted)
                .Activity(context => context.OfInstanceType<EncodeDegradeActivity>())
                .Finalize());

        During(EncodeSubmitRequest?.Pending, Queued,
            When(EncodeStarted)
                .Activity(context => context.OfType<EncodeStartedActivity>())
                .TransitionTo(Processing));

        During(Processing,
            When(EncodeHeartbeat)
                .Activity(context => context.OfType<EncodeHeartbeatActivity>()),
            When(EncodeProgressed)
                .Activity(context => context.OfType<EncodeProgressedActivity>()),
            When(EncodeCompleted)
                .Finalize());

        DuringAny(
            When(EncodeFailed)
                .Finalize());

        // DuringAny(
        //     When(Cancelled)
        //         .Activity(context => context.OfInstanceType<TranscodeCancelledActivity>())
        //         .Finalize());

        // DuringAny(
        //     When(Cancellation)
        //         .IfElse(context => context.Saga.JobId != null,
        //             context => context
        //                 .PublishAsync(x => x.Init<CancelJob>(new { x.Saga.JobId }))
        //                 .TransitionTo(Cancelling),
        //             context => context
        //                 .Activity(x => x.OfInstanceType<TranscodeCancelledActivity>())
        //                 .Finalize()));

        SetCompletedWhenFinalized();
    }

    public required State Queued { get; set; }
    public required State Processing { get; set; }

    public required Event<FileEncodeCreatedEvent> EncodeCreated { get; set; }
    public required Event<EncodeStartedEvent> EncodeStarted { get; set; }
    public required Event<EncodeCompletedEvent> EncodeCompleted { get; set; }
    public required Event<EncodeFailedEvent> EncodeFailed { get; set; }
    public required Event<EncodeHeartbeatEvent> EncodeHeartbeat { get; set; }
    public required Event<EncodeProgressedEvent> EncodeProgressed { get; set; }

    public required Request<EncodeSagaState, EncodeSubmit.Command, EncodeSubmit.Result> EncodeSubmitRequest { get; set; }
}