using EntityFramework.Exceptions.Common;
using Giantnodes.Infrastructure;
using Giantnodes.Infrastructure.Faults;
using Giantnodes.Infrastructure.Uow.Services;
using Giantnodes.Service.Orchestrator.Application.Contracts.Recipes;
using Giantnodes.Service.Orchestrator.Domain.Aggregates.Recipes;
using Giantnodes.Service.Orchestrator.Domain.Aggregates.Recipes.Repositories;
using Giantnodes.Service.Orchestrator.Domain.Enumerations;
using MassTransit;
using Npgsql;

namespace Giantnodes.Service.Orchestrator.Application.Components.Recipes.Commands;

public class RecipeCreateConsumer : IConsumer<RecipeCreate.Command>
{
    private readonly IUnitOfWorkService _uow;
    private readonly IRecipeRepository _repository;

    public RecipeCreateConsumer(IUnitOfWorkService uow, IRecipeRepository repository)
    {
        _uow = uow;
        _repository = repository;
    }

    public async Task Consume(ConsumeContext<RecipeCreate.Command> context)
    {
        using var uow = await _uow.BeginAsync(context.CancellationToken);

        VideoFileContainer? container = null;
        if (context.Message.Container.HasValue && !Enumeration.TryParse<VideoFileContainer>(context.Message.Container.Value, out container))
        {
            await context.RejectAsync(FaultKind.NotFound, nameof(context.Message.Container));
            return;
        }

        if (!Enumeration.TryParse<EncodeCodec>(context.Message.Codec, out var codec))
        {
            await context.RejectAsync(FaultKind.NotFound, nameof(context.Message.Codec));
            return;
        }

        if (!Enumeration.TryParse<EncodePreset>(context.Message.Preset, out var preset))
        {
            await context.RejectAsync(FaultKind.NotFound, nameof(context.Message.Preset));
            return;
        }

        EncodeTune? tune = null;
        if (context.Message.Tune.HasValue && !Enumeration.TryParse<EncodeTune>(context.Message.Tune.Value, out tune))
        {
            await context.RejectAsync(FaultKind.NotFound, nameof(context.Message.Tune));
            return;
        }

        var recipe = new Recipe(
            context.Message.Name,
            container,
            codec,
            preset,
            context.Message.UseHardwareAcceleration,
            tune,
            context.Message.Quality);

        try
        {
            _repository.Create(recipe);
            await uow.CommitAsync(context.CancellationToken);
        }
        catch (UniqueConstraintException ex) when (ex.InnerException is PostgresException pg)
        {
            var param = pg.ConstraintName switch
            {
                "ix_recipes_name" => nameof(context.Message.Name),
                _ => null
            };

            await context.RejectAsync(FaultKind.Constraint, param);
            return;
        }

        await context.RespondAsync(new RecipeCreate.Result { Id = recipe.Id });
    }
}