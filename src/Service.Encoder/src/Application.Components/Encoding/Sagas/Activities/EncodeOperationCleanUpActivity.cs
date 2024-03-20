using System.Diagnostics;
using System.IO.Abstractions;
using Giantnodes.Service.Encoder.Persistence.Sagas;
using MassTransit;
using Serilog;

namespace Giantnodes.Service.Encoder.Application.Components.Encoding.Sagas.Activities;

public class EncodeOperationCleanUpActivity : IStateMachineActivity<EncodeOperationSagaState>
{
    private readonly IFileSystem _fs;

    public EncodeOperationCleanUpActivity(IFileSystem fs)
    {
        _fs = fs;
    }

    public void Probe(ProbeContext context)
    {
        context.CreateScope(KebabCaseEndpointNameFormatter.Instance.Message<EncodeOperationCleanUpActivity>());
    }

    public void Accept(StateMachineVisitor visitor)
    {
        visitor.Visit(this);
    }

    public async Task Execute(
        BehaviorContext<EncodeOperationSagaState> context,
        IBehavior<EncodeOperationSagaState> next)
    {
        Execute(context);
        await next.Execute(context);
    }

    public async Task Execute<T>(
        BehaviorContext<EncodeOperationSagaState, T> context,
        IBehavior<EncodeOperationSagaState, T> next)
        where T : class
    {
        Execute(context);
        await next.Execute(context);
    }

    public Task Faulted<TException>(
        BehaviorExceptionContext<EncodeOperationSagaState, TException> context,
        IBehavior<EncodeOperationSagaState> next)
        where TException : Exception
    {
        return next.Faulted(context);
    }

    public Task Faulted<T, TException>(
        BehaviorExceptionContext<EncodeOperationSagaState, T, TException> context,
        IBehavior<EncodeOperationSagaState, T> next)
        where T : class
        where TException : Exception
    {
        return next.Faulted(context);
    }

    private void Execute(SagaConsumeContext<EncodeOperationSagaState> context)
    {
        var tmp = _fs.FileInfo.New(context.Saga.TempFilePath);
        if (!tmp.Exists)
            return;

        var stopwatch = new Stopwatch();

        stopwatch.Start();
        tmp.Delete();
        stopwatch.Stop();

        Log.Information("Successfully removed {0} with job id {1} in {2:000ms} due to operation failure.", context.Saga.TempFilePath, context.Saga.JobId, stopwatch.ElapsedMilliseconds);
    }
}