using Giantnodes.Infrastructure.Faults.Exceptions;
using Giantnodes.Infrastructure.Faults.Types;
using Giantnodes.Infrastructure.Validation.Exceptions;
using Giantnodes.Service.Dashboard.Application.Contracts.Recipes;
using Giantnodes.Service.Dashboard.Domain.Aggregates.Recipes;
using Giantnodes.Service.Dashboard.Persistence.DbContexts;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace Giantnodes.Service.Dashboard.HttpApi.Resolvers.Recipes.Mutations;

[ExtendObjectType(OperationTypeNames.Mutation)]
public class RecipeUpdateMutation
{
    [Error<DomainException>]
    [Error<ValidationException>]
    [UseFirstOrDefault]
    [UseProjection]
    public async Task<IQueryable<Recipe>> RecipeUpdate(
        [Service] ApplicationDbContext database,
        [Service] IRequestClient<RecipeUpdate.Command> request,
        [ID] Guid id,
        string name,
        [ID] int? container,
        [ID] int codec,
        [ID] int preset,
        [ID] int? tune,
        int? quality,
        bool use_hardware_acceleration,
        CancellationToken cancellation = default)
    {
        var command = new RecipeUpdate.Command
        {
            Id = id,
            Name = name,
            Container = container,
            Codec = codec,
            Preset = preset,
            Tune = tune,
            Quality = quality,
            UseHardwareAcceleration = use_hardware_acceleration
        };

        Response response = await request.GetResponse<RecipeUpdate.Result, DomainFault, ValidationFault>(command, cancellation);
        return response switch
        {
            (_, RecipeUpdate.Result result) => database.Recipes.AsNoTracking().Where(x => x.Id == result.Id),
            (_, DomainFault fault) => throw new DomainException(fault),
            (_, ValidationFault fault) => throw new ValidationException(fault),
            _ => throw new InvalidOperationException()
        };
    }
}