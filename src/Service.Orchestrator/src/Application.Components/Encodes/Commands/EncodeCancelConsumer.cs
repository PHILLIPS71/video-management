using Giantnodes.Infrastructure.Faults;
using Giantnodes.Infrastructure.Uow.Services;
using Giantnodes.Service.Orchestrator.Application.Contracts.Encodes.Commands;
using Giantnodes.Service.Orchestrator.Domain.Aggregates.Encodes.Repositories;
using Giantnodes.Service.Orchestrator.Domain.Shared.Enums;
using MassTransit;

namespace Giantnodes.Service.Orchestrator.Application.Components.Encodes.Commands;

public class EncodeCancelConsumer : IConsumer<EncodeCancel.Command>
{
    private readonly IUnitOfWorkService _uow;
    private readonly IEncodeRepository _repository;

    public EncodeCancelConsumer(IUnitOfWorkService uow, IEncodeRepository repository)
    {
        _uow = uow;
        _repository = repository;
    }

    public async Task Consume(ConsumeContext<EncodeCancel.Command> context)
    {
        using var uow = await _uow.BeginAsync(context.CancellationToken);

        var encode = await _repository.SingleOrDefaultAsync(x => x.Id == context.Message.EncodeId);
        if (encode == null)
        {
            await context.RejectAsync(FaultKind.NotFound, nameof(context.Message.EncodeId));
            return;
        }

        encode.SetCancelled(InVar.Timestamp);

        await uow.CommitAsync(context.CancellationToken);
        await context.RespondAsync(new EncodeCancel.Result { EncodeId = encode.Id });
    }
}