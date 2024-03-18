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
public class EncodeProfileUpdateMutation
{
    [Error<DomainException>]
    [Error<ValidationException>]
    [UseFirstOrDefault]
    [UseProjection]
    public async Task<IQueryable<EncodeProfile>> EncodeProfileUpdate(
        [Service] ApplicationDbContext database,
        [Service] IRequestClient<EncodeProfileUpdate.Command> request,
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
        var command = new EncodeProfileUpdate.Command
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

        Response response = await request.GetResponse<EncodeProfileUpdate.Result, DomainFault, ValidationFault>(command, cancellation);
        return response switch
        {
            (_, EncodeProfileUpdate.Result result) => database.EncodeProfiles.AsNoTracking().Where(x => x.Id == result.Id),
            (_, DomainFault fault) => throw new DomainException(fault),
            (_, ValidationFault fault) => throw new ValidationException(fault),
            _ => throw new InvalidOperationException()
        };
    }
}