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
        string path,
        CancellationToken cancellation = default)
    {
        var command = new DirectoryProbe.Command
        {
            FullPath = path,
        };

        Response response = await request.GetResponse<DirectoryProbe.Result, DomainFault, ValidationFault>(command, cancellation);
        return response switch
        {
            (_, DirectoryProbe.Result result) => result.FullPath,
            (_, DomainFault fault) => throw new DomainException(fault),
            (_, ValidationFault fault) => throw new ValidationException(fault),
            _ => throw new InvalidOperationException()
        };
    }
}