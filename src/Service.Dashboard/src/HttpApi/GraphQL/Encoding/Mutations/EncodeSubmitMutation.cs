using Giantnodes.Infrastructure.Exceptions;
using Giantnodes.Infrastructure.MassTransit.Validation.Messages;
using Giantnodes.Service.Dashboard.Application.Contracts.Encoding.Commands;
using Giantnodes.Service.Dashboard.Domain.Entities.Encoding;
using Giantnodes.Service.Dashboard.Persistence.DbContexts;
using MassTransit;

namespace Giantnodes.Service.Dashboard.HttpApi.GraphQL.Encoding.Mutations;

[ExtendObjectType(OperationTypeNames.Mutation)]
public class EncodeSubmitMutation
{
    [UseFirstOrDefault]
    [UseProjection]
    public async Task<IQueryable<Encode>> EncodeSubmit(
        [Service] ApplicationDbContext database,
        [Service] IRequestClient<EncodeSubmit.Command> request,
        Guid presetId,
        string fullPath,
        CancellationToken cancellation = default)
    {
        var command = new EncodeSubmit.Command { PresetId = presetId, FullPath = fullPath };

        Response response = await request.GetResponse<EncodeSubmit.Result, EncodeSubmit.Rejected, ValidationResult>(command, cancellation);
        return response switch
        {
            (_, EncodeSubmit.Result result) => database.Encodes.Where(x => x.Id == result.Id),
            (_, EncodeSubmit.Rejected error) => throw new DomainException<EncodeSubmit.Rejected>(error),
            (_, ValidationFault error) => throw new DomainException<ValidationFault>(error),
            _ => throw new InvalidOperationException()
        };
    }
}