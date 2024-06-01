using Giantnodes.Infrastructure.Faults;
using Giantnodes.Infrastructure.Uow.Services;
using Giantnodes.Service.Orchestrator.Application.Contracts.Encodes.Commands;
using Giantnodes.Service.Orchestrator.Domain.Aggregates.Entries.Directories;
using Giantnodes.Service.Orchestrator.Domain.Aggregates.Entries.Files;
using Giantnodes.Service.Orchestrator.Domain.Aggregates.Entries.Repositories;
using Giantnodes.Service.Orchestrator.Domain.Aggregates.Recipes.Repositories;
using MassTransit;

namespace Giantnodes.Service.Orchestrator.Application.Components.Encodes.Commands;

public class EncodeSubmitConsumer : IConsumer<EncodeSubmit.Command>
{
    private readonly IUnitOfWorkService _uow;
    private readonly IFileSystemEntryRepository _entries;
    private readonly IRecipeRepository _recipes;

    public EncodeSubmitConsumer(
        IUnitOfWorkService uow,
        IFileSystemEntryRepository entries,
        IRecipeRepository recipes)
    {
        _uow = uow;
        _entries = entries;
        _recipes = recipes;
    }

    public async Task Consume(ConsumeContext<EncodeSubmit.Command> context)
    {
        using var uow = await _uow.BeginAsync(context.CancellationToken);

        var entries = await _entries.ToListAsync(x => context.Message.Entries.Contains(x.Id));
        if (entries.Count == 0)
        {
            await context.RejectAsync(FaultKind.NotFound, nameof(context.Message.Entries));
            return;
        }

        var recipe = await _recipes.SingleOrDefaultAsync(x => x.Id == context.Message.RecipeId);
        if (recipe == null)
        {
            await context.RejectAsync(FaultKind.NotFound, nameof(context.Message.RecipeId));
            return;
        }

        var files = new List<FileSystemFile>();
        foreach (var entry in entries)
        {
            switch (entry)
            {
                case FileSystemFile file:
                    files.Add(file);
                    break;

                case FileSystemDirectory directory:
                    files.AddRange(directory.Entries.OfType<FileSystemFile>());
                    break;
            }
        }

        var encodes = files
            .Select(file => file.Encode(recipe))
            .ToList();

        await uow.CommitAsync(context.CancellationToken);

        var ids = encodes.Select(x => x.Id).ToArray();
        await context.RespondAsync(new EncodeSubmit.Result { Encodes = ids });
    }
}