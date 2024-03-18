using Giantnodes.Infrastructure.Faults.Exceptions;
using Giantnodes.Infrastructure.Faults.Types;
using Giantnodes.Infrastructure.Validation.Exceptions;
using Giantnodes.Service.Dashboard.Application.Contracts.EncodeProfiles;
using Giantnodes.Service.Dashboard.Domain.Aggregates.EncodeProfiles;
using Giantnodes.Service.Dashboard.Persistence.DbContexts;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace Giantnodes.Service.Dashboard.HttpApi.Resolvers.EncodeProfiles.Mutations;

[ExtendObjectType(OperationTypeNames.Mutation)]
public class EncodeProfileDeleteMutation
{
    [Error<DomainException>]
    [Error<ValidationException>]
    [UseFirstOrDefault]
    [UseProjection]
    public async Task<IQueryable<EncodeProfile>> EncodeProfileDelete(
        [Service] ApplicationDbContext database,
        [Service] IRequestClient<EncodeProfileDelete.Command> request,
        [ID] Guid id,
        CancellationToken cancellation = default)
    {
        var command = new EncodeProfileDelete.Command
        {
            Id = id,
        };

        Response response = await request.GetResponse<EncodeProfileDelete.Result, DomainFault, ValidationFault>(command, cancellation);
        return response switch
        {
            (_, EncodeProfileDelete.Result result) => database.EncodeProfiles.IgnoreQueryFilters().AsNoTracking().Where(x => x.Id == result.Id),
            (_, DomainFault fault) => throw new DomainException(fault),
            (_, ValidationFault fault) => throw new ValidationException(fault),
            _ => throw new InvalidOperationException()
        };
    }
}