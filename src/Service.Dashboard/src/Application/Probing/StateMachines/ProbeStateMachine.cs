using Giantnodes.Service.Dashboard.Application.Contracts.Probing.Events;
using Giantnodes.Service.Dashboard.Application.Probing.StateMachines.Activities;
using Giantnodes.Service.Dashboard.Persistence.Sagas;
using MassTransit;
using MassTransit.Contracts.JobService;

namespace Giantnodes.Service.Dashboard.Application.Probing.StateMachines;

public class ProbeStateMachine : MassTransitStateMachine<ProbeSaga>
{
    public ProbeStateMachine()
    {
        InstanceState(x => x.CurrentState);
        
        Event(() => Submitted, e => e.CorrelateById(x => x.Message.Id));
        Event(() => JobStarted, e => e.CorrelateBy((instance, context) => instance.JobId == context.Message.JobId));
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
                .Activity(context => context.OfType<ProbePrepareActivity>())
                .TransitionTo(Queued));
        
        During(Queued,
            When(JobStarted)
                .Activity(context => context.OfType<ProbeStartedActivity>())
                .TransitionTo(Started));
        
        During(Queued, Started,
            When(JobCompleted)
                .Activity(context => context.OfType<ProbeCompletedActivity>())
                .TransitionTo(Completed)
                .Finalize(),
            When(JobFaulted)
                .Activity(context => context.OfType<ProbeFaultedActivity>())
                .TransitionTo(Completed)
                .Finalize(),
            When(JobCancelled)
                .Activity(context => context.OfType<ProbeCancelledActivity>())
                .TransitionTo(Cancelled)
                .Finalize());
        
        SetCompletedWhenFinalized();
    }

    public required Event<ProbeSubmittedEvent> Submitted { get; set; }
    public required Event<JobStarted> JobStarted { get; set; }
    public required Event<JobCompleted> JobCompleted { get; set; }
    public required Event<JobFaulted> JobFaulted { get; set; }
    public required Event<JobCanceled> JobCancelled { get; set; }
    
    public required State Queued { get; set; }
    public required State Started { get; set; }
    public required State Cancelled { get; set; }
    public required State Completed { get; set; }
}