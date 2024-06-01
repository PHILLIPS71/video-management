using Giantnodes.Infrastructure.Faults.Exceptions;
using Giantnodes.Infrastructure.Faults.Types;
using Giantnodes.Infrastructure.Validation.Exceptions;
using Giantnodes.Service.Orchestrator.Application.Contracts.Libraries.Commands;
using Giantnodes.Service.Orchestrator.Domain.Aggregates.Libraries;
using Giantnodes.Service.Orchestrator.Persistence.DbContexts;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace Giantnodes.Service.Orchestrator.HttpApi.Resolvers.Libraries.Mutations;

[MutationType]
internal sealed class LibraryCreateMutation
{
    [Error<DomainException>]
    [Error<ValidationException>]
    [UseFirstOrDefault]
    [UseProjection]
    public async Task<IQueryable<Library>> LibraryCreate(
        [Service] ApplicationDbContext database,
        [Service] IRequestClient<LibraryCreate.Command> request,
        string name,
        string slug,
        string path,
        bool is_watched,
        CancellationToken cancellation = default)
    {
        var command = new LibraryCreate.Command
        {
            Name = name,
            Slug = slug,
            DirectoryPath = path,
            IsWatched = false
        };

        Response response = await request.GetResponse<LibraryCreate.Result, DomainFault, ValidationFault>(command, cancellation);
        return response switch
        {
            (_, LibraryCreate.Result result) => database.Libraries.AsNoTracking().Where(x => x.Id == result.Id),
            (_, DomainFault fault) => throw new DomainException(fault),
            (_, ValidationFault fault) => throw new ValidationException(fault),
            _ => throw new InvalidOperationException()
        };
    }
}