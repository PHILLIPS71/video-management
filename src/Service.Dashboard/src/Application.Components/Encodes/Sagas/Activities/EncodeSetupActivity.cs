using System.IO.Abstractions;
using Giantnodes.Service.Dashboard.Application.Contracts.Encodes.Events;
using Giantnodes.Service.Dashboard.Persistence.Sagas;
using MassTransit;

namespace Giantnodes.Service.Dashboard.Application.Components.Encodes.Sagas.Activities;

public class EncodeSetupActivity : IStateMachineActivity<EncodeSagaState, EncodeCreatedEvent>
{
    private readonly IFileSystem _fs;

    public EncodeSetupActivity(IFileSystem fs)
    {
        _fs = fs;
    }

    public void Probe(ProbeContext context)
    {
        context.CreateScope(KebabCaseEndpointNameFormatter.Instance.Message<EncodeSetupActivity>());
    }

    public void Accept(StateMachineVisitor visitor)
    {
        visitor.Visit(this);
    }

    public Task Execute(
        BehaviorContext<EncodeSagaState, EncodeCreatedEvent> context,
        IBehavior<EncodeSagaState, EncodeCreatedEvent> next)
    {
        var file = _fs.FileInfo.New(context.Message.FilePath);
        if (!file.Exists)
            throw new FileNotFoundException(new FileNotFoundException().Message, context.Message.FilePath);

        var output = file.DirectoryName;
        if (output == null)
            throw new DirectoryNotFoundException($"The directory of {context.Message.FilePath} cannot be found.");

        context.Saga.EncodeId = context.Message.EncodeId;
        context.Saga.InputFilePath = context.Message.FilePath;
        context.Saga.OutputDirectoryPath = output;

        return next.Execute(context);
    }

    public Task Faulted<TException>(
        BehaviorExceptionContext<EncodeSagaState, EncodeCreatedEvent, TException> context,
        IBehavior<EncodeSagaState, EncodeCreatedEvent> next)
        where TException : Exception
    {
        return next.Faulted(context);
    }
}