using EntityFramework.Exceptions.Common;
using Giantnodes.Infrastructure;
using Giantnodes.Infrastructure.Faults;
using Giantnodes.Infrastructure.Uow.Services;
using Giantnodes.Service.Dashboard.Application.Contracts.Recipes;
using Giantnodes.Service.Dashboard.Domain.Aggregates.Recipes.Repositories;
using Giantnodes.Service.Dashboard.Domain.Enumerations;
using MassTransit;
using Npgsql;

namespace Giantnodes.Service.Dashboard.Application.Components.Recipes.Commands;

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
        if (context.Message.Container.HasValue &&
            !Enumeration.TryParse<VideoFileContainer>(context.Message.Container.Value, out container))
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