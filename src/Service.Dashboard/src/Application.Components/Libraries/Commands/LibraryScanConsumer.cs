using Giantnodes.Infrastructure.Faults;
using Giantnodes.Infrastructure.Uow.Services;
using Giantnodes.Service.Dashboard.Application.Contracts.Libraries.Commands;
using Giantnodes.Service.Dashboard.Domain.Aggregates.Libraries.Repositories;
using Giantnodes.Service.Dashboard.Domain.Aggregates.Libraries.Services;
using MassTransit;

namespace Giantnodes.Service.Dashboard.Application.Components.Libraries.Commands;

public class LibraryScanConsumer : IConsumer<LibraryScan.Command>
{
    private readonly IUnitOfWorkService _uow;
    private readonly ILibraryRepository _repository;
    private readonly IFileSystemService _fileSystemService;

    public LibraryScanConsumer(
        IUnitOfWorkService uow,
        ILibraryRepository repository,
        IFileSystemService fileSystemService)
    {
        _uow = uow;
        _repository = repository;
        _fileSystemService = fileSystemService;
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

            library.Scan(_fileSystemService);
            await uow.CommitAsync(context.CancellationToken);

            await context.RespondAsync(new LibraryCreate.Result { Id = library.Id });
        }
    }
}