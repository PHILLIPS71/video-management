using Giantnodes.Infrastructure.Faults;
using Giantnodes.Infrastructure.Uow.Services;
using Giantnodes.Service.Dashboard.Application.Contracts.Encodes.Commands;
using Giantnodes.Service.Dashboard.Application.Contracts.Encodes.Events;
using Giantnodes.Service.Dashboard.Domain.Aggregates.Encodes.Repositories;
using Giantnodes.Service.Dashboard.Domain.Shared.Enums;
using MassTransit;

namespace Giantnodes.Service.Dashboard.Application.Components.Encodes.Commands;

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
        using (var uow = await _uow.BeginAsync(context.CancellationToken))
        {
            var encode = await _repository.SingleOrDefaultAsync(x => x.Id == context.Message.EncodeId);
            if (encode == null)
            {
                await context.RejectAsync(FaultKind.NotFound, nameof(context.Message.EncodeId));
                return;
            }

            encode.SetStatus(EncodeStatus.Cancelled);

            await context.Publish(new EncodeCancelledEvent { EncodeId = encode.Id }, context.CancellationToken);
            await uow.CommitAsync(context.CancellationToken);

            await context.RespondAsync(new EncodeCancel.Result { EncodeId = encode.Id });
        }
    }
}