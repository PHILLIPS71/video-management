using System.IO.Abstractions;
using Giantnodes.Service.Dashboard.Application.Contracts.Encodes.Events;
using Giantnodes.Service.Dashboard.Domain.Aggregates.EncodeProfiles.Repositories;
using Giantnodes.Service.Dashboard.Persistence.Sagas;
using MassTransit;

namespace Giantnodes.Service.Dashboard.Application.Components.Encodes.Sagas.Activities;

public class EncodeSetupActivity : IStateMachineActivity<EncodeSagaState, EncodeCreatedEvent>
{
    private readonly IFileSystem _fs;
    private readonly IEncodeProfileRepository _repository;

    public EncodeSetupActivity(IFileSystem fs, IEncodeProfileRepository repository)
    {
        _fs = fs;
        _repository = repository;
    }

    public void Probe(ProbeContext context)
    {
        context.CreateScope(KebabCaseEndpointNameFormatter.Instance.Message<EncodeSetupActivity>());
    }

    public void Accept(StateMachineVisitor visitor)
    {
        visitor.Visit(this);
    }

    public async Task Execute(
        BehaviorContext<EncodeSagaState, EncodeCreatedEvent> context,
        IBehavior<EncodeSagaState, EncodeCreatedEvent> next)
    {
        var file = _fs.FileInfo.New(context.Message.FilePath);
        if (!file.Exists)
            throw new FileNotFoundException(new FileNotFoundException().Message, context.Message.FilePath);

        var output = file.DirectoryName;
        if (output == null)
            throw new DirectoryNotFoundException($"The directory of {context.Message.FilePath} cannot be found.");

        var profile = await _repository.SingleAsync(x => x.Id == context.Message.EncodeProfileId);

        context.Saga.OutputFilePath = context.Message.FilePath;
        if (profile.Container != null)
            context.Saga.OutputFilePath = _fs.Path.ChangeExtension(context.Message.FilePath, profile.Container.Extension);

        context.Saga.InputFilePath = context.Message.FilePath;
        context.Saga.EncodeId = context.Message.EncodeId;
        context.Saga.Codec = profile.Codec;
        context.Saga.Preset = profile.Preset;
        context.Saga.Tune = profile.Tune;
        context.Saga.Quality = profile.Quality;
        context.Saga.UseHardwareAcceleration = profile.UseHardwareAcceleration;

        await next.Execute(context);
    }

    public Task Faulted<TException>(
        BehaviorExceptionContext<EncodeSagaState, EncodeCreatedEvent, TException> context,
        IBehavior<EncodeSagaState, EncodeCreatedEvent> next)
        where TException : Exception
    {
        return next.Faulted(context);
    }
}