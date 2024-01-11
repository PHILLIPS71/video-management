using Giantnodes.Service.Encoder.Application.Contracts.Encoding.Commands;
using Giantnodes.Service.Encoder.Application.Contracts.Encoding.Events;
using Giantnodes.Service.Encoder.Application.Contracts.Encoding.Jobs;
using Giantnodes.Service.Encoder.Persistence.Sagas;
using MassTransit;
using MassTransit.Contracts.JobService;

namespace Giantnodes.Service.Encoder.Application.Components.Encoding.Sagas;

public class EncodeJobStateMachine : MassTransitStateMachine<EncodeJobSaga>
{
    public EncodeJobStateMachine()
    {
        InstanceState(x => x.CurrentState);

        Event(() => Submitted);
        Event(() => Started);
        Event(() => Completed);
        Event(() => Faulted, e => e.CorrelateById(context => context.Message.JobId));
        Event(() => Cancelled);
        
        Request(() => Encode);

        Initially(
            When(Submitted)
                .Then(context =>
                {
                    context.Saga.InputPath = context.Message.InputPath;
                    context.Saga.IsDeletingInput = context.Message.IsDeletingInput;
                    context.Saga.OutputDirectoryPath = context.Message.OutputDirectoryPath;
                })
                .Request(Encode, context => new Encode.Job { FullPath = context.Saga.InputPath })
                .RespondAsync(context => context.Init<EncodeSubmit.Result>(new { context.Saga.CorrelationId }))
                .TransitionTo(Encode?.Pending));

        During(Encode?.Pending,
            When(Encode?.Completed)
                .Then(context => context.Saga.JobId = context.Message.JobId)
                .TransitionTo(Queued),
            When(Encode?.TimeoutExpired)
                .If(context => context.Saga.JobId == null,
                    context => context.PublishAsync(ctx => ctx.Init<EncodeFailedEvent>(new { ctx.Saga.CorrelationId }))),
            When(Encode?.Faulted)
                .Finalize());

        During(Encode?.Pending, Queued,
            When(Started)
                .Then(context => context.Saga.OutputTempPath = context.Message.OutputPath)
                .TransitionTo(Encoding));

        During(Encoding,
            When(Completed)
                .TransitionTo(Transferring)
                .Finalize());

        DuringAny(
            When(Cancelled)
                .If(context => context.Saga.JobId != null,
                    context => context.PublishAsync(ctx => ctx.Init<CancelJob>(new { ctx.Saga.JobId })))
                .Finalize(),
            When(Faulted)
                .PublishAsync(context => context.Init<EncodeFailedEvent>(new { context.Saga.CorrelationId, context.Message.Exceptions }))
                .Finalize());

        SetCompletedWhenFinalized();
    }

    public required State Queued { get; set; }
    public required State Encoding { get; set; }
    public required State Transferring { get; set; }

    public required Event<EncodeSubmit.Command> Submitted { get; set; }
    public required Event<EncodeStartedEvent> Started { get; set; }
    public required Event<JobFaulted> Faulted { get; set; }
    public required Event<EncodeCompletedEvent> Completed { get; set; }
    public required Event<EncodeCancel.Command> Cancelled { get; set; }

    public required Request<EncodeJobSaga, Encode.Job, JobSubmissionAccepted> Encode { get; set; }
}