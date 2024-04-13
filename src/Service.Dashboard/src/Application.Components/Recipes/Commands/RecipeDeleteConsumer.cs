using Giantnodes.Infrastructure.Faults;
using Giantnodes.Infrastructure.Uow.Services;
using Giantnodes.Service.Dashboard.Application.Contracts.Recipes;
using Giantnodes.Service.Dashboard.Domain.Aggregates.Recipes.Repositories;
using MassTransit;

namespace Giantnodes.Service.Dashboard.Application.Components.Recipes.Commands;

public class RecipeDeleteConsumer : IConsumer<RecipeDelete.Command>
{
    private readonly IUnitOfWorkService _uow;
    private readonly IRecipeRepository _repository;

    public RecipeDeleteConsumer(IUnitOfWorkService uow, IRecipeRepository repository)
    {
        _uow = uow;
        _repository = repository;
    }

    public async Task Consume(ConsumeContext<RecipeDelete.Command> context)
    {
        using var uow = await _uow.BeginAsync(context.CancellationToken);

        var recipe = await _repository.SingleOrDefaultAsync(x => x.Id == context.Message.Id);
        if (recipe == null)
        {
            await context.RejectAsync(FaultKind.NotFound, nameof(context.Message.Id));
            return;
        }

        _repository.Delete(recipe);
        await uow.CommitAsync(context.CancellationToken);
        await context.RespondAsync(new RecipeDelete.Result { Id = recipe.Id });
    }
}