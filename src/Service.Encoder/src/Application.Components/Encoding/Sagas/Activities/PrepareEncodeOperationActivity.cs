using System.IO.Abstractions;
using Giantnodes.Service.Encoder.Application.Contracts.Encoding.Commands;
using Giantnodes.Service.Encoder.Persistence.Sagas;
using MassTransit;

namespace Giantnodes.Service.Encoder.Application.Components.Encoding.Sagas.Activities;

public class PrepareEncodeOperationActivity
    : IStateMachineActivity<EncodeOperationSagaState, EncodeOperationSubmit.Command>
{
    private readonly IFileSystem _fs;

    public PrepareEncodeOperationActivity(IFileSystem fs)
    {
        _fs = fs;
    }

    public void Probe(ProbeContext context)
    {
        context.CreateScope(KebabCaseEndpointNameFormatter.Instance.Message<PrepareEncodeOperationActivity>());
    }

    public void Accept(StateMachineVisitor visitor)
    {
        visitor.Visit(this);
    }

    public async Task Execute(
        BehaviorContext<EncodeOperationSagaState, EncodeOperationSubmit.Command> context,
        IBehavior<EncodeOperationSagaState, EncodeOperationSubmit.Command> next)
    {
        var file = _fs.FileInfo.New(context.Message.InputFilePath);
        if (!file.Exists)
            throw new FileNotFoundException(new FileNotFoundException().Message, context.Message.InputFilePath);

        var container = file.Extension;
        if (!string.IsNullOrWhiteSpace(context.Message.Container))
            container = context.Message.Container;

        context.Saga.InputFilePath = context.Message.InputFilePath;
        context.Saga.OutputFilePath = GetPath(context.Message.OutputDirectoryPath, file.Name, container);
        context.Saga.TempFilePath = GetPath(_fs.Path.GetTempPath(), context.Saga.CorrelationId.ToString(), container);

        await next.Execute(context);
    }

    public Task Faulted<TException>(
        BehaviorExceptionContext<EncodeOperationSagaState, EncodeOperationSubmit.Command, TException> context,
        IBehavior<EncodeOperationSagaState, EncodeOperationSubmit.Command> next)
        where TException : Exception
    {
        return next.Faulted(context);
    }

    /// <summary>
    /// Concatenates the specified directory, name, and container to form a complete file path. The file extension of
    /// the resulting path is modified based on the specified container parameter.
    /// </summary>
    /// <param name="directory">The directory part of the file path.</param>
    /// <param name="name">The name of the file.</param>
    /// <param name="container">The container or file extension to be used for modifying the file extension.</param>
    /// <returns>A string representing the complete file path formed by joining directory, modified name, and container.</returns>
    private string GetPath(string directory, string name, string container)
    {
        return _fs.Path.Join(directory, _fs.Path.ChangeExtension(name, container));
    }
}