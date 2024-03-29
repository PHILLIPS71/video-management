﻿using Giantnodes.Service.Encoder.Application.Components.Encoding.Sagas.Activities;
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

        Event(() => Started,
            e => e.CorrelateBy((instance, context) => instance.JobId == context.Message.JobId));
        Event(() => Encoded,
            e => e.CorrelateBy((instance, context) => instance.JobId == context.Message.JobId));
        Event(() => Transferred,
            e => e.CorrelateBy((instance, context) => instance.JobId == context.Message.JobId));
        Event(() => Faulted,
            e => e.CorrelateBy((instance, context) => instance.JobId == context.Message.JobId));

        Event(() => Cancellation);

        Initially(
            When(Submitted)
                .Activity(context => context.OfType<PrepareEncodeOperationActivity>())
                .RequestEncodeFile()
                .TransitionTo(Queued));

        During(Queued,
            When(Started)
                .PublishOperationStarted()
                .TransitionTo(Encoding));

        During(Encoding,
            When(Encoded)
                .RequestTransferFile()
                .TransitionTo(Transferring));

        During(Transferring,
            When(Transferred)
                .PublishOperationCompleted()
                .Finalize());

        DuringAny(
            When(Cancellation)
                .If(context => context.Saga.JobId != null,
                    context => context.PublishAsync(ctx => ctx.Init<CancelJob>(new { ctx.Saga.JobId })))
                .Finalize(),
            When(Faulted)
                .PublishOperationFailed()
                .Activity(context => context.OfInstanceType<EncodeOperationCleanUpActivity>())
                .Finalize());

        During(Encoding,
            Ignore(Started));

        SetCompletedWhenFinalized();
    }

    public required State Queued { get; set; }
    public required State Encoding { get; set; }
    public required State Transferring { get; set; }
    public required Event<EncodeOperationSubmit.Command> Submitted { get; set; }
    public required Event<JobStarted<EncodeFile.Job>> Started { get; set; }
    public required Event<JobCompleted<EncodeFile.Job>> Encoded { get; set; }
    public required Event<JobCompleted<TransferFile.Job>> Transferred { get; set; }
    public required Event<JobFaulted> Faulted { get; set; }
    public required Event<EncodeOperationCancel.Command> Cancellation { get; set; }
}

internal static class EncodeOperationStateBehaviorExtensions
{
    public static EventActivityBinder<EncodeOperationSagaState, TEvent> PublishOperationStarted<TEvent>(
        this EventActivityBinder<EncodeOperationSagaState, TEvent> binder)
        where TEvent : class
    {
        return binder
            .PublishAsync(context => context.Init<EncodeOperationStartedEvent>(
                new EncodeOperationStartedEvent
                {
                    CorrelationId = context.Saga.CorrelationId
                }));
    }

    public static EventActivityBinder<EncodeOperationSagaState, EncodeOperationSubmit.Command> RequestEncodeFile(
        this EventActivityBinder<EncodeOperationSagaState, EncodeOperationSubmit.Command> binder)
    {
        return binder
            .Then(context => context.Saga.JobId = NewId.NextSequentialGuid())
            .PublishAsync(context => context.Init<SubmitJob<EncodeFile.Job>>(new
            {
                context.Saga.JobId,
                Job = new EncodeFile.Job
                {
                    CorrelationId = context.Saga.CorrelationId,
                    InputFilePath = context.Saga.InputFilePath,
                    OutputFilePath = context.Saga.TempFilePath,
                    Codec = context.Message.Codec,
                    Preset = context.Message.Preset,
                    Tune = context.Message.Tune,
                    Quality = context.Message.Quality,
                    UseHardwareAcceleration = context.Message.UseHardwareAcceleration
                }
            }));
    }

    public static EventActivityBinder<EncodeOperationSagaState, TEvent> RequestTransferFile<TEvent>(
        this EventActivityBinder<EncodeOperationSagaState, TEvent> binder)
        where TEvent : class
    {
        return binder
            .Then(context => context.Saga.JobId = NewId.NextSequentialGuid())
            .PublishAsync(context => context.Init<SubmitJob<TransferFile.Job>>(new
            {
                context.Saga.JobId,
                Job = new TransferFile.Job
                {
                    CorrelationId = context.Saga.CorrelationId,
                    InputFilePath = context.Saga.TempFilePath,
                    OutputFilePath = context.Saga.OutputFilePath
                }
            }));
    }

    public static EventActivityBinder<EncodeOperationSagaState, JobFaulted> PublishOperationFailed(
        this EventActivityBinder<EncodeOperationSagaState, JobFaulted> binder)
    {
        return binder
            .PublishAsync(context => context.Init<EncodeOperationFailedEvent>(
                new EncodeOperationFailedEvent
                {
                    CorrelationId = context.Saga.CorrelationId,
                    Exceptions = context.Message.Exceptions
                }));
    }

    public static EventActivityBinder<EncodeOperationSagaState, TEvent> PublishOperationCompleted<TEvent>(
        this EventActivityBinder<EncodeOperationSagaState, TEvent> binder)
        where TEvent : class
    {
        return binder
            .PublishAsync(context => context.Init<EncodeOperationCompletedEvent>(
                new EncodeOperationCompletedEvent
                {
                    CorrelationId = context.Saga.CorrelationId,
                    InputFilePath = context.Saga.InputFilePath,
                    OutputFilePath = context.Saga.OutputFilePath
                }));
    }
}