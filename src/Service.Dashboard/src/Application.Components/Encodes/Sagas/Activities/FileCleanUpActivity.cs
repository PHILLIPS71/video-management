using System.Diagnostics;
using System.IO.Abstractions;
using Giantnodes.Service.Dashboard.Persistence.Sagas;
using MassTransit;
using Serilog;

namespace Giantnodes.Service.Dashboard.Application.Components.Encodes.Sagas.Activities;

public class FileCleanUpActivity : IStateMachineActivity<EncodeSagaState>
{
    private readonly IFileSystem _fs;

    public FileCleanUpActivity(IFileSystem fs)
    {
        _fs = fs;
    }

    public void Probe(ProbeContext context)
    {
        context.CreateScope(KebabCaseEndpointNameFormatter.Instance.Message<FileCleanUpActivity>());
    }

    public void Accept(StateMachineVisitor visitor)
    {
        visitor.Visit(this);
    }

    public Task Execute(BehaviorContext<EncodeSagaState> context, IBehavior<EncodeSagaState> next)
    {
        Execute(context);
        return next.Execute(context);
    }

    public Task Execute<T>(BehaviorContext<EncodeSagaState, T> context, IBehavior<EncodeSagaState, T> next)
        where T : class
    {
        Execute(context);
        return next.Execute(context);
    }

    public Task Faulted<TException>(
        BehaviorExceptionContext<EncodeSagaState, TException> context,
        IBehavior<EncodeSagaState> next)
        where TException : Exception
    {
        return next.Faulted(context);
    }

    public Task Faulted<T, TException>(
        BehaviorExceptionContext<EncodeSagaState, T, TException> context,
        IBehavior<EncodeSagaState, T> next)
        where T : class
        where TException : Exception
    {
        return next.Faulted(context);
    }

    private void Execute(SagaConsumeContext<EncodeSagaState> context)
    {
        var file = _fs.FileInfo.New(context.Saga.InputFilePath);
        if (!file.Exists)
            return;

        if (string.IsNullOrWhiteSpace(context.Saga.OutputFilePath))
            return;

        // prevent deleting the file if the full path of the output file is the same as the full path of the input file
        if (string.Equals(file.FullName, _fs.Path.GetFullPath(context.Saga.OutputFilePath), StringComparison.OrdinalIgnoreCase))
            return;

        var stopwatch = new Stopwatch();

        stopwatch.Start();
        _fs.FileStream.New(file.FullName, FileMode.Open, FileAccess.Read, FileShare.None, 4096, FileOptions.DeleteOnClose | FileOptions.Asynchronous);
        stopwatch.Stop();

        Log.Information("Successfully deleted source file {0} for encode {1} in {2:000ms}", file.FullName, context.Saga.EncodeId, stopwatch.ElapsedMilliseconds);
    }
}