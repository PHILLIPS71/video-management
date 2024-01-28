using System.IO.Abstractions;
using Giantnodes.Infrastructure.Uow.Services;
using Giantnodes.Service.Dashboard.Application.Contracts.Libraries.Events;
using Giantnodes.Service.Dashboard.Domain.Aggregates.Libraries.Repositories;
using MassTransit;

namespace Giantnodes.Service.Dashboard.Application.Components.Libraries.Events;

public class OnFileSystemChangedScan : IConsumer<Batch<LibraryFileSystemChangedEvent>>
{
    private readonly IFileSystem _fs;
    private readonly IUnitOfWorkService _uow;
    private readonly ILibraryRepository _repository;

    public OnFileSystemChangedScan(
        IFileSystem fs,
        IUnitOfWorkService uow,
        ILibraryRepository repository)
    {
        _fs = fs;
        _uow = uow;
        _repository = repository;
    }

    public async Task Consume(ConsumeContext<Batch<LibraryFileSystemChangedEvent>> context)
    {
        using (var uow = await _uow.BeginAsync(context.CancellationToken))
        {
            var libraries = await _repository
                .ToListAsync(x => context.Message.Select(y => y.Message.LibraryId).Contains(x.Id), context.CancellationToken);

            foreach (var library in libraries)
            {
                library.Scan(_fs);
            }

            await uow.CommitAsync(context.CancellationToken);
        }
    }
}