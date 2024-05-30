using Giantnodes.Infrastructure.Faults.Exceptions;
using Giantnodes.Infrastructure.Faults.Types;
using Giantnodes.Infrastructure.Validation.Exceptions;
using Giantnodes.Service.Dashboard.Application.Contracts.Libraries.Commands;
using Giantnodes.Service.Dashboard.Domain.Aggregates.Libraries;
using Giantnodes.Service.Dashboard.Persistence.DbContexts;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace Giantnodes.Service.Dashboard.HttpApi.Resolvers.Libraries.Mutations;

[MutationType]
internal sealed class LibraryUpdateMutation
{
    [Error<DomainException>]
    [Error<ValidationException>]
    [UseFirstOrDefault]
    [UseProjection]
    public async Task<IQueryable<Library>> LibraryUpdate(
        [Service] ApplicationDbContext database,
        [Service] IRequestClient<LibraryUpdate.Command> request,
        [ID] Guid id,
        string name,
        string slug,
        bool is_watched,
        CancellationToken cancellation = default)
    {
        var command = new LibraryUpdate.Command
        {
            Id = id,
            Name = name,
            Slug = slug,
            IsWatched = is_watched
        };

        Response response = await request.GetResponse<LibraryUpdate.Result, DomainFault, ValidationFault>(command, cancellation);
        return response switch
        {
            (_, LibraryUpdate.Result result) => database.Libraries.AsNoTracking().Where(x => x.Id == result.Id),
            (_, DomainFault fault) => throw new DomainException(fault),
            (_, ValidationFault fault) => throw new ValidationException(fault),
            _ => throw new InvalidOperationException()
        };
    }
}