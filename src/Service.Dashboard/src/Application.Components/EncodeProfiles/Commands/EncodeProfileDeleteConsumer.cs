using Giantnodes.Infrastructure.Faults;
using Giantnodes.Infrastructure.Uow.Services;
using Giantnodes.Service.Dashboard.Application.Contracts.EncodeProfiles;
using Giantnodes.Service.Dashboard.Domain.Aggregates.EncodeProfiles.Repositories;
using MassTransit;

namespace Giantnodes.Service.Dashboard.Application.Components.EncodeProfiles.Commands;

public class EncodeProfileDeleteConsumer : IConsumer<EncodeProfileDelete.Command>
{
    private readonly IUnitOfWorkService _uow;
    private readonly IEncodeProfileRepository _repository;

    public EncodeProfileDeleteConsumer(IUnitOfWorkService uow, IEncodeProfileRepository repository)
    {
        _uow = uow;
        _repository = repository;
    }

    public async Task Consume(ConsumeContext<EncodeProfileDelete.Command> context)
    {
        using var uow = await _uow.BeginAsync(context.CancellationToken);

        var profile = await _repository.SingleOrDefaultAsync(x => x.Id == context.Message.Id);
        if (profile == null)
        {
            await context.RejectAsync(FaultKind.NotFound, nameof(context.Message.Id));
            return;
        }

        _repository.Delete(profile);
        await uow.CommitAsync(context.CancellationToken);
        await context.RespondAsync(new EncodeProfileDelete.Result { Id = profile.Id });
    }
}