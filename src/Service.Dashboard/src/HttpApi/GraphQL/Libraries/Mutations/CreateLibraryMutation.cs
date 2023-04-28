using Giantnodes.Infrastructure.Exceptions;
using Giantnodes.Infrastructure.MassTransit.Validation.Messages;
using Giantnodes.Service.Dashboard.Application.Contracts.Libraries.Commands;
using MassTransit;

namespace Giantnodes.Service.Dashboard.HttpApi.GraphQL.Libraries.Mutations;

[ExtendObjectType(OperationTypeNames.Mutation)]
public class CreateLibraryMutation
{
    public async Task<Guid> CreateLibrary(
        [Service] IRequestClient<CreateLibrary.Command> request,
        string name,
        string slug,
        string fullPath,
        CancellationToken cancellation = default)
    {
        var command = new CreateLibrary.Command
        {
            Name = name, 
            Slug = slug, 
            FullPath = fullPath
        };

        Response response = await request.GetResponse<CreateLibrary.Result, CreateLibrary.Rejected, ValidationResult>(command, cancellation);
        return response switch
        {
            (_, CreateLibrary.Result result) => result.Id,
            (_, CreateLibrary.Rejected error) => throw new DomainException<CreateLibrary.Rejected>(error),
            (_, ValidationFault error) => throw new DomainException<ValidationFault>(error),
            _ => throw new InvalidOperationException()
        };
    }
}