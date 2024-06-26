using EntityFramework.Exceptions.Common;
using Giantnodes.Infrastructure;
using Giantnodes.Infrastructure.Faults;
using Giantnodes.Infrastructure.Uow.Services;
using Giantnodes.Service.Orchestrator.Application.Contracts.Recipes;
using Giantnodes.Service.Orchestrator.Domain.Aggregates.Recipes.Repositories;
using Giantnodes.Service.Orchestrator.Domain.Enumerations;
using MassTransit;
using Npgsql;

namespace Giantnodes.Service.Orchestrator.Application.Components.Recipes.Commands;

public class RecipeUpdateConsumer : IConsumer<RecipeUpdate.Command>
{
    private readonly IUnitOfWorkService _uow;
    private readonly IRecipeRepository _repository;

    public RecipeUpdateConsumer(IUnitOfWorkService uow, IRecipeRepository repository)
    {
        _uow = uow;
        _repository = repository;
    }

    public async Task Consume(ConsumeContext<RecipeUpdate.Command> context)
    {
        using var uow = await _uow.BeginAsync(context.CancellationToken);

        var recipe = await _repository.SingleOrDefaultAsync(x => x.Id == context.Message.Id, context.CancellationToken);
        if (recipe == null)
        {
            await context.RejectAsync(FaultKind.NotFound, nameof(context.Message.Id));
            return;
        }

        VideoFileContainer? container = null;
        if (context.Message.Container.HasValue)
        {
            container = Enumeration.TryParse<VideoFileContainer>(context.Message.Container.Value);
            if (container == null)
            {
                await context.RejectAsync(FaultKind.NotFound, nameof(context.Message.Container));
                return;
            }
        }

        var codec = Enumeration.TryParse<EncodeCodec>(context.Message.Codec);
        if (codec == null)
        {
            await context.RejectAsync(FaultKind.NotFound, nameof(context.Message.Codec));
            return;
        }

        var preset = Enumeration.TryParse<EncodePreset>(context.Message.Preset);
        if (preset == null)
        {
            await context.RejectAsync(FaultKind.NotFound, nameof(context.Message.Preset));
            return;
        }

        EncodeTune? tune = null;
        if (context.Message.Tune.HasValue)
        {
            tune = Enumeration.TryParse<EncodeTune>(context.Message.Tune.Value);
            if (tune == null)
            {
                await context.RejectAsync(FaultKind.NotFound, nameof(context.Message.Tune));
                return;
            }
        }

        recipe.SetName(context.Message.Name);
        recipe.SetContainer(container);
        recipe.SetCodec(codec);
        recipe.SetPreset(preset);
        recipe.SetTune(tune);
        recipe.SetQuality(context.Message.Quality);
        recipe.SetUseHardwareAcceleration(context.Message.UseHardwareAcceleration);

        try
        {
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

        await context.RespondAsync(new RecipeUpdate.Result { Id = recipe.Id });
    }
}