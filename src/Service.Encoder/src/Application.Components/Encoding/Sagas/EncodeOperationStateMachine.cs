using Giantnodes.Service.Encoder.Application.Contracts.Encoding.Commands;
using Giantnodes.Service.Encoder.Application.Contracts.Encoding.Events;
using Giantnodes.Service.Encoder.Application.Contracts.Encoding.Jobs;
using Giantnodes.Service.Encoder.Persistence.Sagas;
using MassTransit;
using MassTransit.Contracts.JobService;

namespace Giantnodes.Service.Encoder.Application.Components.Encoding.Sagas;

public class EncodeOperationStateMachine : MassTransitStateMachine<EncodeOperationSagaState>
{
    public EncodeOperationStateMachine()
    {
        InstanceState(x => x.CurrentState);

        Event(() => Submitted);
        Event(() => Started, e => e.CorrelateBy((instance, context) => instance.JobId == context.Message.JobId));
        Event(() => Completed, e => e.CorrelateBy((instance, context) => instance.JobId == context.Message.JobId));
        Event(() => Faulted, e => e.CorrelateBy((instance, context) => instance.JobId == context.Message.JobId));
        Event(() => Cancellation);

        Initially(
            When(Submitted)
                .Then(context =>
                {
                    context.Saga.JobId = NewId.NextSequentialGuid();
                    context.Saga.FilePath = context.Message.FilePath;
                })
                .PublishAsync(context => context.Init<SubmitJob<EncodeFile.Job>>(new
                {
                    JobId = context.Saga.JobId,
                    Job = new EncodeFile.Job
                    {
                        CorrelationId = context.Saga.CorrelationId,
                        FilePath = context.Message.FilePath
                    }
                }))
                .TransitionTo(Queued));

        During(Queued,
            When(Started)
                .PublishAsync(context => context.Init<EncodeOperationStartedEvent>(new EncodeOperationStartedEvent
                {
                    CorrelationId = context.Saga.CorrelationId
                }))
                .TransitionTo(Encoding));

        During(Encoding,
            When(Completed)
                .Then(context => context.Saga.JobId = null)
                .PublishAsync(context => context.Init<EncodeOperationCompletedEvent>(new EncodeOperationCompletedEvent
                {
                    CorrelationId = context.Saga.CorrelationId,
                    InputFilePath = context.Saga.FilePath,
                    OutputFilePath = context.Saga.FilePath
                }))
                .TransitionTo(Transferring)
                .Finalize());

        DuringAny(
            When(Cancellation)
                .If(context => context.Saga.JobId != null,
                    context => context.PublishAsync(ctx => ctx.Init<CancelJob>(new { ctx.Saga.JobId })))
                .Finalize(),
            When(Faulted)
                .PublishAsync(context => context.Init<EncodeOperationFailedEvent>(new EncodeOperationFailedEvent
                {
                    CorrelationId = context.Saga.CorrelationId,
                    Exceptions = context.Message.Exceptions
                }))
                .Finalize());

        SetCompletedWhenFinalized();
    }

    public required State Queued { get; set; }
    public required State Encoding { get; set; }
    public required State Transferring { get; set; }

    public required Event<EncodeOperationSubmit.Command> Submitted { get; set; }
    public required Event<JobStarted> Started { get; set; }
    public required Event<JobFaulted> Faulted { get; set; }
    public required Event<JobCompleted> Completed { get; set; }
    public required Event<EncodeOperationCancel.Command> Cancellation { get; set; }
}