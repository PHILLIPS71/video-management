using Giantnodes.Service.Dashboard.Application.Components.Files.Sagas.Activities;
using Giantnodes.Service.Dashboard.Application.Contracts.Transcoding.Commands;
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

        Event(() => Submitted, e => e.CorrelateById(context => context.Message.FileId));
        Event(() => JobStarted, e => e.CorrelateBy((instance, context) => instance.JobId == context.Message.JobId));
        Event(() => JobFaulted, e => e.CorrelateBy((instance, context) => instance.JobId == context.Message.JobId));
        Event(() => JobCompleted, e => e.CorrelateBy((instance, context) => instance.JobId == context.Message.JobId));
        Event(() => JobProgressed, e => e.CorrelateBy((instance, context) => instance.JobId == context.Message.JobId));

        Request(() => Encode);

        Initially(
            When(Submitted)
                .Then(context => context.Saga.SubmittedAt = DateTime.UtcNow)
                .Activity(context => context.OfType<TranscodePrepareActivity>())
                .Request(Encode, context => new Transcode.Job { FullPath = context.Saga.InputFullPath })
                .RespondAsync(context => context.Init<FileTranscode.Result>(new FileTranscode.Result { TranscodeId = context.Saga.CorrelationId }))
                .TransitionTo(Encode?.Pending));

        During(Encode?.Pending,
            When(Encode?.Completed)
                .Then(context => context.Saga.JobId = context.Message.JobId)
                .TransitionTo(Queued),
            When(Encode?.TimeoutExpired)
                .TransitionTo(Failed)
                .Finalize(),
            When(Encode?.Faulted)
                .TransitionTo(Failed)
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
                .TransitionTo(Failed)
                .Finalize());

        SetCompletedWhenFinalized();
    }

    public required State Queued { get; set; }
    public required State Started { get; set; }
    public required State Completed { get; set; }
    public required State Failed { get; set; }

    public required Event<FileTranscode.Command> Submitted { get; set; }
    public required Event<JobStarted> JobStarted { get; set; }
    public required Event<JobFaulted> JobFaulted { get; set; }
    public required Event<JobCompleted> JobCompleted { get; set; }
    public required Event<TranscodeProgressedEvent> JobProgressed { get; set; }

    public required Request<TranscodeSagaState, Transcode.Job, JobSubmissionAccepted> Encode { get; set; }
}