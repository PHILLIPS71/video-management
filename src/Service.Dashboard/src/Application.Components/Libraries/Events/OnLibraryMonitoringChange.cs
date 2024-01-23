using Giantnodes.Infrastructure.Uow.Services;
using Giantnodes.Service.Dashboard.Application.Contracts.Libraries.Events;
using Giantnodes.Service.Dashboard.Domain.Aggregates.Libraries.Repositories;
using Giantnodes.Service.Dashboard.Domain.Aggregates.Libraries.Services;
using MassTransit;

namespace Giantnodes.Service.Dashboard.Application.Components.Libraries.Events;

public class OnLibraryMonitoringChange : IConsumer<LibraryMonitoringChangedEvent>
{
    private readonly ILibraryRepository _repository;
    private readonly IUnitOfWorkService _uow;
    private readonly ILibraryMonitoringService _monitoring;

    public OnLibraryMonitoringChange(
        IUnitOfWorkService uow,
        ILibraryRepository repository,
        ILibraryMonitoringService monitoring)
    {
        _uow = uow;
        _repository = repository;
        _monitoring = monitoring;
    }

    public async Task Consume(ConsumeContext<LibraryMonitoringChangedEvent> context)
    {
        using (var uow = await _uow.BeginAsync(context.CancellationToken))
        {
            var library = await _repository.SingleOrDefaultAsync(x => x.Id == context.Message.LibraryId, context.CancellationToken);
            if (library == null)
                return;

            switch (context.Message.IsMonitoring)
            {
                case true:
                    await _monitoring.TryMonitorAsync(library);
                    break;
                
                case false:
                    _monitoring.TryUnMonitor(library);
                    break;
            }

            await uow.CommitAsync(context.CancellationToken);
        }
    }
}