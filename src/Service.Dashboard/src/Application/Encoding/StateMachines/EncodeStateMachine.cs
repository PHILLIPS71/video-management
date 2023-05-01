using Giantnodes.Service.Dashboard.Application.Contracts.Encoding.Events;
using Giantnodes.Service.Dashboard.Application.Encoding.StateMachines.Activities;
using Giantnodes.Service.Dashboard.Persistence.Sagas;
using Giantnodes.Service.Encoder.Application.Contracts.Encoding.Events;
using MassTransit;
using MassTransit.Contracts.JobService;

namespace Giantnodes.Service.Dashboard.Application.Encoding.StateMachines;

public class EncodeStateMachine : MassTransitStateMachine<EncodeSaga>
{
    public EncodeStateMachine()
    {
        InstanceState(x => x.CurrentState);

        Event(() => Submitted, e => e.CorrelateById(x => x.Message.Id));
        Event(() => JobStarted, e => e.CorrelateBy((instance, context) => instance.JobId == context.Message.JobId));
        Event(() => JobProgressed, e => e.CorrelateBy((instance, context) => instance.JobId == context.Message.JobId));
        Event(() => JobCompleted, e => e.CorrelateBy((instance, context) => instance.JobId == context.Message.JobId));
        Event(() => JobFaulted, e => e.CorrelateBy((instance, context) => instance.JobId == context.Message.JobId));
        Event(() => JobCancelled, e => e.CorrelateBy((instance, context) => instance.JobId == context.Message.JobId));

        Initially(
            When(Submitted)
                .Then(context =>
                {
                    context.Saga.FullPath = context.Message.FullPath;
                    context.Saga.SubmittedAt = DateTime.UtcNow;
                })
                .Activity(context => context.OfType<EncodePrepareActivity>())
                .TransitionTo(Queued));

        During(Queued,
            When(JobStarted)
                .Activity(context => context.OfType<EncodeStartedActivity>())
                .TransitionTo(Started));

        During(Queued, Started,
            When(JobProgressed)
                .Activity(context => context.OfType<EncodeProgressedActivity>()),
            When(JobCompleted)
                .Activity(context => context.OfType<EncodeCompletedActivity>())
                .TransitionTo(Completed)
                .Finalize(),
            When(JobFaulted)
                .Activity(context => context.OfType<EncodeFaultedActivity>())
                .TransitionTo(Completed)
                .Finalize(),
            When(JobCancelled)
                // .Activity(context => context.OfType<EncodeCancelledActivity>())
                .TransitionTo(Cancelled)
                .Finalize());

        SetCompletedWhenFinalized();
    }

    public required Event<EncodeSubmittedEvent> Submitted { get; set; }
    public required Event<JobStarted> JobStarted { get; set; }
    public required Event<EncodeFileProgressed> JobProgressed { get; set; }
    public required Event<JobCompleted> JobCompleted { get; set; }
    public required Event<JobFaulted> JobFaulted { get; set; }
    public required Event<JobCanceled> JobCancelled { get; set; }

    public required State Queued { get; set; }
    public required State Started { get; set; }
    public required State Cancelled { get; set; }
    public required State Completed { get; set; }
}