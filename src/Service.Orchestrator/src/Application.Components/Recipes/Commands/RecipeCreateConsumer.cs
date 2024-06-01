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