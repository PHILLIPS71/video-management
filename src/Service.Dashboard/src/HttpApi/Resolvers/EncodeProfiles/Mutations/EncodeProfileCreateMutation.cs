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
public class EncodeProfileCreateMutation
{
    [Error<DomainException>]
    [Error<ValidationException>]
    [UseFirstOrDefault]
    [UseProjection]
    public async Task<IQueryable<EncodeProfile>> EncodeProfileCreate(
        [Service] ApplicationDbContext database,
        [Service] IRequestClient<EncodeProfileCreate.Command> request,
        string name,
        [ID] int? container,
        [ID] int codec,
        [ID] int preset,
        [ID] int? tune,
        int? quality,
        CancellationToken cancellation = default)
    {
        var command = new EncodeProfileCreate.Command
        {
            Name = name,
            Container = container,
            Codec = codec,
            Preset = preset,
            Tune = tune,
            Quality = quality,
        };

        Response response = await request.GetResponse<EncodeProfileCreate.Result, DomainFault, ValidationFault>(command, cancellation);
        return response switch
        {
            (_, EncodeProfileCreate.Result result) => database.EncodeProfiles.AsNoTracking().Where(x => x.Id == result.Id),
            (_, DomainFault fault) => throw new DomainException(fault),
            (_, ValidationFault fault) => throw new ValidationException(fault),
            _ => throw new InvalidOperationException()
        };
    }
}