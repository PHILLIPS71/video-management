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

        context.Saga.InputFilePath = context.Message.InputFilePath;
        context.Saga.OutputFilePath = context.Message.OutputFilePath;
        context.Saga.TempFilePath = _fs.Path.Join(
            _fs.Path.GetTempPath(),
            string.Join(context.Saga.CorrelationId.ToString(), _fs.Path.GetExtension(context.Message.OutputFilePath)));

        await next.Execute(context);
    }

    public Task Faulted<TException>(
        BehaviorExceptionContext<EncodeOperationSagaState, EncodeOperationSubmit.Command, TException> context,
        IBehavior<EncodeOperationSagaState, EncodeOperationSubmit.Command> next)
        where TException : Exception
    {
        return next.Faulted(context);
    }
}