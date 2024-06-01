using System.IO.Abstractions;
using Giantnodes.Infrastructure.Faults;
using Giantnodes.Infrastructure.Uow.Services;
using Giantnodes.Service.Orchestrator.Application.Contracts.Libraries.Commands;
using Giantnodes.Service.Orchestrator.Domain.Aggregates.Libraries.Repositories;
using MassTransit;

namespace Giantnodes.Service.Orchestrator.Application.Components.Libraries.Commands;

public class LibraryScanConsumer : IConsumer<LibraryScan.Command>
{
    private readonly IFileSystem _fs;
    private readonly IUnitOfWorkService _uow;
    private readonly ILibraryRepository _repository;

    public LibraryScanConsumer(
        IFileSystem fs,
        IUnitOfWorkService uow,
        ILibraryRepository repository)
    {
        _fs = fs;
        _uow = uow;
        _repository = repository;
    }

    public async Task Consume(ConsumeContext<LibraryScan.Command> context)
    {
        using (var uow = await _uow.BeginAsync(context.CancellationToken))
        {
            var library = await _repository.SingleOrDefaultAsync(x => x.Id == context.Message.LibraryId, context.CancellationToken);
            if (library == null)
            {
                await context.RejectAsync(FaultKind.NotFound, nameof(context.Message.LibraryId));
                return;
            }

            library.Scan(_fs);
            await uow.CommitAsync(context.CancellationToken);

            await context.RespondAsync(new LibraryCreate.Result { Id = library.Id });
        }
    }
}