using Giantnodes.Service.Dashboard.Application.Components.Files.Sagas.Activities;
using Giantnodes.Service.Dashboard.Application.Contracts.Files.Events;
using Giantnodes.Service.Dashboard.Persistence.Sagas;
using Giantnodes.Service.Encoder.Application.Contracts.Encoding.Events;
using Giantnodes.Service.Encoder.Application.Contracts.Encoding.Jobs;
using MassTransit;
using MassTransit.Contracts.JobService;

namespace Giantnodes.Service.Dashboard.Application.Components.Files.Sagas;

public class TranscodeStateMachine : MassTransitStateMachine<TranscodeSagaState>
{
    public TranscodeStateMachine()
    {
        InstanceState(x => x.CurrentState);

        Event(() => Submitted, e => e.CorrelateById(context => context.Message.TranscodeId));
        Event(() => Cancellation, e => e.CorrelateById(context => context.Message.TranscodeId));
        Event(() => JobStarted, e => e.CorrelateBy((instance, context) => instance.JobId == context.Message.JobId));
        Event(() => Cancelled, e => e.CorrelateBy((instance, context) => instance.JobId == context.Message.JobId));
        Event(() => JobFaulted, e => e.CorrelateBy((instance, context) => instance.JobId == context.Message.JobId));
        Event(() => JobCompleted, e => e.CorrelateBy((instance, context) => instance.JobId == context.Message.JobId));
        Event(() => JobProgressed, e => e.CorrelateBy((instance, context) => instance.JobId == context.Message.JobId));

        Request(() => Encode);

        Initially(
            When(Submitted)
                .Then(context =>
                {
                    context.Saga.InputFullPath = context.Message.FullPath;
                    context.Saga.SubmittedAt = DateTime.UtcNow;
                })
                .Request(Encode, context => new Transcode.Job { FullPath = context.Saga.InputFullPath })
                .TransitionTo(Encode?.Pending));

        During(Encode?.Pending,
            When(Encode?.Completed)
                .Then(context => context.Saga.JobId = context.Message.JobId)
                .Activity(context => context.OfType<TranscodeQueuedActivity>())
                .TransitionTo(Queued),
            When(Encode?.TimeoutExpired)
                .If(context => context.Saga.JobId == null,
                    context => context.Activity(x => x.OfInstanceType<TranscodeDegradeActivity>())),
            When(Encode?.Faulted)
                .Finalize());

        During(Encode?.Pending, Queued,
            When(JobStarted)
                .Then(context => context.Saga.JobId = context.Message.JobId)
                .Activity(context => context.OfType<TranscodeStartedActivity>())
                .TransitionTo(Started));

        During(Started,
            When(JobProgressed)
                .Activity(context => context.OfType<TranscodeProgressedActivity>()),
            When(JobCompleted)
                .Activity(context => context.OfType<TranscodeCompletedActivity>())
                .TransitionTo(Completed)
                .Finalize());

        DuringAny(
            When(JobFaulted)
                .Finalize());

        DuringAny(
            When(Cancelled)
                .Activity(context => context.OfInstanceType<TranscodeCancelledActivity>())
                .Finalize());

        DuringAny(
            When(Cancellation)
                .IfElse(context => context.Saga.JobId != null,
                    context => context
                        .PublishAsync(x => x.Init<CancelJob>(new { x.Saga.JobId }))
                        .TransitionTo(Cancelling),
                    context => context
                        .Activity(x => x.OfInstanceType<TranscodeCancelledActivity>())
                        .Finalize()));

        SetCompletedWhenFinalized();
    }

    public required State Queued { get; set; }
    public required State Started { get; set; }
    public required State Cancelling { get; set; }
    public required State Completed { get; set; }

    public required Event<FileTranscodeCreatedEvent> Submitted { get; set; }
    public required Event<FileTranscodeCancellationEvent> Cancellation { get; set; }
    public required Event<JobCanceled> Cancelled { get; set; }
    public required Event<JobStarted> JobStarted { get; set; }
    public required Event<JobFaulted> JobFaulted { get; set; }
    public required Event<JobCompleted> JobCompleted { get; set; }
    public required Event<TranscodeProgressedEvent> JobProgressed { get; set; }

    public required Request<TranscodeSagaState, Transcode.Job, JobSubmissionAccepted> Encode { get; set; }
}