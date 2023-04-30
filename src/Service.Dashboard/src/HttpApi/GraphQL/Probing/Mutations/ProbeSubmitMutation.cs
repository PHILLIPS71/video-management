using Giantnodes.Infrastructure.Exceptions;
using Giantnodes.Infrastructure.MassTransit.Validation.Messages;
using Giantnodes.Service.Dashboard.Application.Contracts.Probing.Commands;
using Giantnodes.Service.Dashboard.Domain.Entities.Probing;
using Giantnodes.Service.Dashboard.Persistence.DbContexts;
using MassTransit;

namespace Giantnodes.Service.Dashboard.HttpApi.GraphQL.Probing.Mutations;

[ExtendObjectType(OperationTypeNames.Mutation)]
public class ProbeSubmitMutation
{
    [UseFirstOrDefault]
    [UseProjection]
    public async Task<IQueryable<Probe>> ProbeSubmit(
        [Service] ApplicationDbContext database,
        [Service] IRequestClient<ProbeSubmit.Command> request,
        string fullPath,
        CancellationToken cancellation = default)
    {
        var command = new ProbeSubmit.Command { FullPath = fullPath };

        Response response = await request.GetResponse<ProbeSubmit.Result, ProbeSubmit.Rejected, ValidationResult>(command, cancellation);
        return response switch
        {
            (_, ProbeSubmit.Result result) => database.Probes.Where(x => x.Id == result.Id),
            (_, ProbeSubmit.Rejected error) => throw new DomainException<ProbeSubmit.Rejected>(error),
            (_, ValidationFault error) => throw new DomainException<ValidationFault>(error),
            _ => throw new InvalidOperationException()
        };
    }
}