using System.IO.Abstractions;
using EntityFramework.Exceptions.Common;
using Giantnodes.Infrastructure.Faults;
using Giantnodes.Infrastructure.Uow.Services;
using Giantnodes.Service.Orchestrator.Application.Contracts.Libraries.Commands;
using Giantnodes.Service.Orchestrator.Domain.Aggregates.Libraries;
using Giantnodes.Service.Orchestrator.Domain.Aggregates.Libraries.Repositories;
using MassTransit;
using Npgsql;

namespace Giantnodes.Service.Orchestrator.Application.Components.Libraries.Commands;

public class LibraryCreateConsumer : IConsumer<LibraryCreate.Command>
{
    private readonly IFileSystem _fs;
    private readonly IUnitOfWorkService _uow;
    private readonly ILibraryRepository _repository;

    public LibraryCreateConsumer(
        IUnitOfWorkService uow,
        IFileSystem fs,
        ILibraryRepository repository)
    {
        _fs = fs;
        _uow = uow;
        _repository = repository;
    }

    public async Task Consume(ConsumeContext<LibraryCreate.Command> context)
    {
        using var uow = await _uow.BeginAsync(context.CancellationToken);

        var directory = _fs.DirectoryInfo.New(context.Message.DirectoryPath);
        if (!directory.Exists)
        {
            await context.RejectAsync(FaultKind.NotFound, nameof(context.Message.DirectoryPath));
            return;
        }

        var library = new Library(directory, context.Message.Name, context.Message.Slug);
        library.Scan(_fs);

        if (context.Message.IsWatched)
            library.SetWatched(context.Message.IsWatched);

        try
        {
            _repository.Create(library);
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

        await context.RespondAsync(new LibraryCreate.Result { Id = library.Id });
    }
}