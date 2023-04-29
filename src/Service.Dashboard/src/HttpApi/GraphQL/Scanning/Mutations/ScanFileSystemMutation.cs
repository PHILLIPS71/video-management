using Giantnodes.Infrastructure.Exceptions;
using Giantnodes.Infrastructure.MassTransit.Validation.Messages;
using Giantnodes.Service.Dashboard.Application.Contracts.Scanning.Commands;
using MassTransit;

namespace Giantnodes.Service.Dashboard.HttpApi.GraphQL.Scanning.Mutations;

[ExtendObjectType(OperationTypeNames.Mutation)]
public class ScanFileSystemMutation
{
    public async Task<string> ScanFileSystem(
        [Service] IRequestClient<ScanFileSystem.Command> request,
        string fullPath,
        CancellationToken cancellation = default)
    {
        var command = new ScanFileSystem.Command { FullPath = fullPath };

        Response response = await request.GetResponse<ScanFileSystem.Result, ScanFileSystem.Rejected, ValidationResult>(command, cancellation);
        return response switch
        {
            (_, ScanFileSystem.Result result) => result.FullPath,
            (_, ScanFileSystem.Rejected error) => throw new DomainException<ScanFileSystem.Rejected>(error),
            (_, ValidationFault error) => throw new DomainException<ValidationFault>(error),
            _ => throw new InvalidOperationException()
        };
    }
}