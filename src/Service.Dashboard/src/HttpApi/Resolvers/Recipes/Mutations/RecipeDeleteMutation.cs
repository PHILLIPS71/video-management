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
public class RecipeDeleteMutation
{
    [Error<DomainException>]
    [Error<ValidationException>]
    [UseFirstOrDefault]
    [UseProjection]
    public async Task<IQueryable<Recipe>> RecipeDelete(
        [Service] ApplicationDbContext database,
        [Service] IRequestClient<RecipeDelete.Command> request,
        [ID] Guid id,
        CancellationToken cancellation = default)
    {
        var command = new RecipeDelete.Command
        {
            Id = id,
        };

        Response response = await request.GetResponse<RecipeDelete.Result, DomainFault, ValidationFault>(command, cancellation);
        return response switch
        {
            (_, RecipeDelete.Result result) => database.Recipes.IgnoreQueryFilters().AsNoTracking().Where(x => x.Id == result.Id),
            (_, DomainFault fault) => throw new DomainException(fault),
            (_, ValidationFault fault) => throw new ValidationException(fault),
            _ => throw new InvalidOperationException()
        };
    }
}