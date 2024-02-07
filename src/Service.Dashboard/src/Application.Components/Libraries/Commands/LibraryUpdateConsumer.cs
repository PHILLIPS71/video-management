using EntityFramework.Exceptions.Common;
using Giantnodes.Infrastructure.Faults;
using Giantnodes.Infrastructure.Uow.Services;
using Giantnodes.Service.Dashboard.Application.Contracts.Libraries.Commands;
using Giantnodes.Service.Dashboard.Domain.Aggregates.Libraries.Repositories;
using MassTransit;
using Npgsql;

namespace Giantnodes.Service.Dashboard.Application.Components.Libraries.Commands;

public class LibraryUpdateConsumer : IConsumer<LibraryUpdate.Command>
{
    private readonly IUnitOfWorkService _uow;
    private readonly ILibraryRepository _repository;

    public LibraryUpdateConsumer(IUnitOfWorkService uow, ILibraryRepository repository)
    {
        _uow = uow;
        _repository = repository;
    }

    public async Task Consume(ConsumeContext<LibraryUpdate.Command> context)
    {
        using var uow = await _uow.BeginAsync(context.CancellationToken);

        var library =
            await _repository.SingleOrDefaultAsync(x => x.Id == context.Message.Id, context.CancellationToken);
        if (library == null)
        {
            await context.RejectAsync(FaultKind.NotFound, nameof(context.Message.Id));
            return;
        }

        try
        {
            if (context.Message.IsWatched)
                library.SetWatched(context.Message.IsWatched);
        }
        catch (PlatformNotSupportedException)
        {
            await context.RejectAsync(FaultKind.Platform);
            return;
        }

        try
        {
            library.SetName(context.Message.Name);
            library.SetSlug(context.Message.Slug);
            await uow.CommitAsync(context.CancellationToken);
        }
        catch (UniqueConstraintException ex) when (ex.InnerException is PostgresException pg)
        {
            var param = pg.ConstraintName switch
            {
                "ix_libraries_slug" => nameof(context.Message.Slug),
                _ => null
            };

            await context.RejectAsync(FaultKind.Constraint, param);
            return;
        }

        await context.RespondAsync(new LibraryUpdate.Result { Id = library.Id });
    }
}