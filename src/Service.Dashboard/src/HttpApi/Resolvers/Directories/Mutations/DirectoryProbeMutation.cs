using Giantnodes.Infrastructure.Faults.Exceptions;
using Giantnodes.Infrastructure.Faults.Types;
using Giantnodes.Infrastructure.Validation.Exceptions;
using Giantnodes.Service.Dashboard.Application.Contracts.Directories.Commands;
using MassTransit;

namespace Giantnodes.Service.Dashboard.HttpApi.Resolvers.Directories.Mutations;

[ExtendObjectType(OperationTypeNames.Mutation)]
public class DirectoryProbeMutation
{
    [Error<DomainException>]
    [Error<ValidationException>]
    [UseFirstOrDefault]
    [UseProjection]
    public async Task<string> DirectoryProbe(
        [Service] IRequestClient<DirectoryProbe.Command> request,
        [ID] Guid directory_id,
        CancellationToken cancellation = default)
    {
        var command = new DirectoryProbe.Command
        {
            DirectoryId = directory_id,
        };

        Response response = await request.GetResponse<DirectoryProbe.Result, DomainFault, ValidationFault>(command, cancellation);
        return response switch
        {
            (_, DirectoryProbe.Result result) => result.FilePath,
            (_, DomainFault fault) => throw new DomainException(fault),
            (_, ValidationFault fault) => throw new ValidationException(fault),
            _ => throw new InvalidOperationException()
        };
    }
}