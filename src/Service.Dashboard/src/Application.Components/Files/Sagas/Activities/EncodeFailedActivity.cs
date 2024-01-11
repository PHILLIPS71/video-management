using Giantnodes.Infrastructure.Uow.Services;
using Giantnodes.Service.Dashboard.Domain.Aggregates.Entries.Files.Repositories;
using Giantnodes.Service.Dashboard.Domain.Shared.Enums;
using Giantnodes.Service.Dashboard.Persistence.Sagas;
using Giantnodes.Service.Encoder.Application.Contracts.Encoding.Events;
using MassTransit;

namespace Giantnodes.Service.Dashboard.Application.Components.Files.Sagas.Activities;

public class EncodeFailedActivity : IStateMachineActivity<EncodeSagaState, EncodeFailedEvent>
{
    private readonly IUnitOfWorkService _uow;
    private readonly IFileSystemFileRepository _repository;

    public EncodeFailedActivity(
        IUnitOfWorkService uow,
        IFileSystemFileRepository repository)
    {
        _uow = uow;
        _repository = repository;
    }

    public void Probe(ProbeContext context)
    {
        context.CreateScope(KebabCaseEndpointNameFormatter.Instance.Message<EncodeCompletedActivity>());
    }

    public void Accept(StateMachineVisitor visitor)
    {
        visitor.Visit(this);
    }

    public async Task Execute(
        BehaviorContext<EncodeSagaState, EncodeFailedEvent> context,
        IBehavior<EncodeSagaState, EncodeFailedEvent> next)
    {
        using (var uow = await _uow.BeginAsync(context.CancellationToken))
        {
            var file = await _repository.SingleAsync(x => x.Encodes.Any(y => y.Id == context.CorrelationId));
            var encode = file.Encodes.Single(x => x.Id == context.CorrelationId);

            encode.SetStatus(EncodeStatus.Failed);
            await uow.CommitAsync(context.CancellationToken);
        }

        await next.Execute(context);
    }

    public Task Faulted<TException>(
        BehaviorExceptionContext<EncodeSagaState, EncodeFailedEvent, TException> context,
        IBehavior<EncodeSagaState, EncodeFailedEvent> next)
        where TException : Exception
    {
        return next.Faulted(context);
    }
}