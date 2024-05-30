using Giantnodes.Infrastructure.Faults.Exceptions;
using Giantnodes.Infrastructure.Faults.Types;
using Giantnodes.Infrastructure.Validation.Exceptions;
using Giantnodes.Service.Dashboard.Application.Contracts.Recipes;
using Giantnodes.Service.Dashboard.Domain.Aggregates.Recipes;
using Giantnodes.Service.Dashboard.Persistence.DbContexts;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace Giantnodes.Service.Dashboard.HttpApi.Resolvers.Recipes.Mutations;

[MutationType]
public class RecipeCreateMutation
{
    [Error<DomainException>]
    [Error<ValidationException>]
    [UseFirstOrDefault]
    [UseProjection]
    public async Task<IQueryable<Recipe>> RecipeCreate(
        [Service] ApplicationDbContext database,
        [Service] IRequestClient<RecipeCreate.Command> request,
        string name,
        [ID] int? container,
        [ID] int codec,
        [ID] int preset,
        [ID] int? tune,
        int? quality,
        bool use_hardware_acceleration,
        CancellationToken cancellation = default)
    {
        var command = new RecipeCreate.Command
        {
            Name = name,
            Container = container,
            Codec = codec,
            Preset = preset,
            Tune = tune,
            Quality = quality,
            UseHardwareAcceleration = use_hardware_acceleration
        };

        Response response = await request.GetResponse<RecipeCreate.Result, DomainFault, ValidationFault>(command, cancellation);
        return response switch
        {
            (_, RecipeCreate.Result result) => database.Recipes.AsNoTracking().Where(x => x.Id == result.Id),
            (_, DomainFault fault) => throw new DomainException(fault),
            (_, ValidationFault fault) => throw new ValidationException(fault),
            _ => throw new InvalidOperationException()
        };
    }
}