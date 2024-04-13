using System.IO.Abstractions;
using Giantnodes.Service.Dashboard.Application.Contracts.Encodes.Events;
using Giantnodes.Service.Dashboard.Domain.Aggregates.Recipes.Repositories;
using Giantnodes.Service.Dashboard.Persistence.Sagas;
using MassTransit;

namespace Giantnodes.Service.Dashboard.Application.Components.Encodes.Sagas.Activities;

public class EncodeSetupActivity : IStateMachineActivity<EncodeSagaState, EncodeCreatedEvent>
{
    private readonly IFileSystem _fs;
    private readonly IRecipeRepository _repository;

    public EncodeSetupActivity(IFileSystem fs, IRecipeRepository repository)
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
        context.Saga.EncodeId = context.Message.EncodeId;

        var file = _fs.FileInfo.New(context.Message.FilePath);
        if (!file.Exists)
            throw new FileNotFoundException(new FileNotFoundException().Message, context.Message.FilePath);

        context.Saga.InputFilePath = context.Message.FilePath;
        context.Saga.OutputFilePath = context.Message.FilePath;

        var recipe = await _repository.SingleAsync(x => x.Id == context.Message.RecipeId);
        if (recipe.Container != null)
            context.Saga.OutputFilePath = _fs.Path.ChangeExtension(context.Message.FilePath, recipe.Container.Extension);

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