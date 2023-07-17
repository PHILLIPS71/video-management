using Giantnodes.Infrastructure.Exceptions;
using Giantnodes.Infrastructure.MassTransit.Validation.Messages;
using Giantnodes.Service.Dashboard.Application.Contracts.Libraries.Commands;
using Giantnodes.Service.Dashboard.Domain.Aggregates.Libraries.Entities;
using Giantnodes.Service.Dashboard.Persistence.DbContexts;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace Giantnodes.Service.Dashboard.HttpApi.Libraries.Mutations;

[ExtendObjectType(OperationTypeNames.Mutation)]
public class LibraryCreateMutation
{
    [UseFirstOrDefault]
    [UseProjection]
    public async Task<IQueryable<Library>> LibraryCreate(
        [Service] ApplicationDbContext database,
        [Service] IRequestClient<LibraryCreate.Command> request,
        string name,
        string slug,
        string path,
        CancellationToken cancellation = default)
    {
        var command = new LibraryCreate.Command
        {
            Name = name,
            Slug = slug,
            FullPath = path
        };

        Response response = await request.GetResponse<LibraryCreate.Result, ValidationResult>(command, cancellation);
        return response switch
        {
            (_, LibraryCreate.Result result) => database.Libraries.AsNoTracking().Where(x => x.Id == result.Id),
            (_, ValidationFault error) => throw new DomainException<ValidationFault>(error),
            _ => throw new InvalidOperationException()
        };
    }
}