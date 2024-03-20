using Giantnodes.Service.Dashboard.Application.Components.Encodes.Sagas.Activities;
using Giantnodes.Service.Dashboard.Application.Contracts.Encodes.Events;
using Giantnodes.Service.Dashboard.Persistence.Sagas;
using Giantnodes.Service.Encoder.Application.Contracts.Encoding.Commands;
using Giantnodes.Service.Encoder.Application.Contracts.Encoding.Events;
using Giantnodes.Service.Encoder.Application.Contracts.Probing.Events;
using Giantnodes.Service.Encoder.Application.Contracts.Probing.Jobs;
using MassTransit;
using MassTransit.Contracts.JobService;

namespace Giantnodes.Service.Dashboard.Application.Components.Encodes.Sagas;

public sealed class EncodeStateMachine : MassTransitStateMachine<EncodeSagaState>
{
    public EncodeStateMachine()
    {
        InstanceState(x => x.CurrentState);

        Event(() => Created);
        Event(() => Started);
        Event(() => Built);
        Event(() => Heartbeat);
        Event(() => Progressed);
        Event(() => Completed);
        Event(() => Failed);

        Event(() => Cancelled,
            e => e.CorrelateBy((instance, context) => instance.EncodeId == context.Message.EncodeId));
        Event(() => FileProbed,
            e => e.CorrelateBy((instance, context) => instance.JobId == context.Message.JobId));

        Initially(
            When(Created)
                .Activity(context => context.OfType<EncodeSetupActivity>())
                .RequestFileProbe(context => context.Saga.InputFilePath)
                .TransitionTo(Submitted));

        During(Submitted,
            When(FileProbed)
                .Then(context => context.Saga.JobId = null)
                .Activity(context => context.OfType<FileProbedActivity>())
                .Activity(context => context.OfInstanceType<EncodeRequestActivity>())
                .TransitionTo(Queued));

        During(Queued,
            When(Started)
                .Activity(context => context.OfType<EncodeOperationStartedActivity>())
                .TransitionTo(Processing));

        During(Processing,
            When(Built)
                .Activity(context => context.OfType<EncodeOperationBuiltActivity>()),
            When(Heartbeat)
                .Activity(context => context.OfType<EncodeOperationHeartbeatActivity>()),
            When(Progressed)
                .Activity(context => context.OfType<EncodeOperationProgressedActivity>()),
            When(Completed)
                .Then(context => context.Saga.OutputFilePath = context.Message.OutputFilePath)
                .Activity(context => context.OfType<EncodeOperationCompletedActivity>())
                .RequestFileProbe(context => context.Message.OutputFilePath)
                .TransitionTo(Encoded));

        During(Encoded,
            When(FileProbed)
                .Then(context => context.Saga.JobId = null)
                .If(context => !context.Saga.IsKeepingSourceFile,
                    context => context.Activity(ctx => ctx.OfInstanceType<FileCleanUpActivity>()))
                .Activity(context => context.OfType<FileProbedActivity>())
                .Finalize());

        DuringAny(
            When(Failed)
                .Activity(context => context.OfType<EncodeOperationFailedActivity>())
                .Finalize(),
            When(Cancelled)
                .RequestOperationCancel()
                .Finalize());

        SetCompletedWhenFinalized();
    }

    public required State Submitted { get; set; }
    public required State Queued { get; set; }
    public required State Processing { get; set; }
    public required State Encoded { get; set; }

    public required Event<EncodeCreatedEvent> Created { get; set; }
    public required Event<FileProbedEvent> FileProbed { get; set; }
    public required Event<EncodeOperationStartedEvent> Started { get; set; }
    public required Event<EncodeOperationEncodeBuiltEvent> Built { get; set; }
    public required Event<EncodeOperationEncodeHeartbeatEvent> Heartbeat { get; set; }
    public required Event<EncodeOperationEncodeProgressedEvent> Progressed { get; set; }
    public required Event<EncodeOperationCompletedEvent> Completed { get; set; }
    public required Event<EncodeCancelledEvent> Cancelled { get; set; }
    public required Event<EncodeOperationFailedEvent> Failed { get; set; }
}

internal static class EncodeStateMachineBehaviorExtensions
{
    public static EventActivityBinder<EncodeSagaState, TEvent> RequestFileProbe<TEvent>(
        this EventActivityBinder<EncodeSagaState, TEvent> binder,
        Func<BehaviorContext<EncodeSagaState, TEvent>, string> factory)
        where TEvent : class
    {
        return binder
            .Then(context => context.Saga.JobId = NewId.NextSequentialGuid())
            .PublishAsync(context => context.Init<SubmitJob<ProbeFileSystem.Job>>(new
            {
                context.Saga.JobId,
                Job = new ProbeFileSystem.Job
                {
                    CorrelationId = context.Saga.CorrelationId,
                    FilePath = factory(context)
                }
            }));
    }

    public static EventActivityBinder<EncodeSagaState, EncodeCancelledEvent> RequestOperationCancel(
        this EventActivityBinder<EncodeSagaState, EncodeCancelledEvent> binder)
    {
        return binder
            .PublishAsync(context => context.Init<EncodeOperationCancel.Command>(new EncodeOperationCancel.Command
            {
                CorrelationId = context.Saga.CorrelationId
            }));
    }
}