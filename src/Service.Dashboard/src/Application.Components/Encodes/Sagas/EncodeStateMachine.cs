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

public class EncodeStateMachine : MassTransitStateMachine<EncodeSagaState>
{
    public EncodeStateMachine()
    {
        InstanceState(x => x.CurrentState);

        Event(() => Submitted);
        Event(() => Cancelled);
        Event(() => Started);
        Event(() => Heartbeat);
        Event(() => Progressed);
        Event(() => Completed);
        Event(() => Failed);
        Event(() => FileProbed, e => e.CorrelateBy((instance, context) => instance.JobId == context.Message.JobId));

        Initially(
            When(Submitted)
                .Then(context =>
                {
                    context.Saga.JobId = NewId.NextSequentialGuid();
                    context.Saga.EncodeId = context.Message.EncodeId;
                    context.Saga.InputFilePath = context.Message.FilePath;
                })
                .PublishAsync(context => context.Init<SubmitJob<ProbeFileSystem.Job>>(new
                {
                    JobId = context.Saga.JobId,
                    Job = new ProbeFileSystem.Job
                    {
                        CorrelationId = context.Saga.CorrelationId,
                        FilePath = context.Message.FilePath
                    }
                }))
                .TransitionTo(Probing));

        During(Probing,
            When(FileProbed)
                .Then(context => context.Saga.JobId = null)
                .Activity(context => context.OfType<FileProbedActivity>())
                .PublishAsync(context => context.Init<EncodeOperationSubmit.Command>(new EncodeOperationSubmit.Command
                {
                    CorrelationId = context.Saga.CorrelationId,
                    FilePath = context.Saga.InputFilePath
                }))
                .TransitionTo(Queued));

        During(Queued,
            When(Started)
                .Activity(context => context.OfType<EncodeStartedActivity>())
                .TransitionTo(Processing));

        During(Processing,
            When(Heartbeat)
                .Activity(context => context.OfType<EncodeHeartbeatActivity>()),
            When(Progressed)
                .Activity(context => context.OfType<EncodeProgressedActivity>()),
            When(Completed)
                .Activity(context => context.OfType<EncodeCompletedActivity>())
                .Then(context =>
                {
                    context.Saga.JobId = NewId.NextSequentialGuid();
                    context.Saga.OutputFilePath = context.Message.OutputFilePath;
                })
                .PublishAsync(context => context.Init<SubmitJob<ProbeFileSystem.Job>>(new
                {
                    JobId = context.Saga.JobId,
                    Job = new ProbeFileSystem.Job
                    {
                        CorrelationId = context.Saga.CorrelationId,
                        FilePath = context.Message.OutputFilePath
                    }
                }))
                .TransitionTo(Encoded));

        During(Encoded,
            When(FileProbed)
                .Then(context => context.Saga.JobId = null)
                .Activity(context => context.OfType<FileProbedActivity>())
                .Finalize());

        DuringAny(
            When(Failed)
                .Activity(context => context.OfType<EncodeFailedActivity>())
                .Finalize(),
            When(Cancelled)
                .PublishAsync(context => context.Init<EncodeOperationCancel.Command>(new EncodeOperationCancel.Command
                {
                    CorrelationId = context.Saga.CorrelationId
                }))
                .Finalize());

        SetCompletedWhenFinalized();
    }

    public required State Queued { get; set; }
    public required State Probing { get; set; }
    public required State Processing { get; set; }
    public required State Encoded { get; set; }

    public required Event<EncodeCreatedEvent> Submitted { get; set; }
    public required Event<FileProbedEvent> FileProbed { get; set; }
    public required Event<EncodeOperationStartedEvent> Started { get; set; }
    public required Event<EncodeOperationCompletedEvent> Completed { get; set; }
    public required Event<EncodeOperationEncodeHeartbeatEvent> Heartbeat { get; set; }
    public required Event<EncodeOperationEncodeProgressedEvent> Progressed { get; set; }
    public required Event<EncodeCancelledEvent> Cancelled { get; set; }
    public required Event<EncodeOperationFailedEvent> Failed { get; set; }
}