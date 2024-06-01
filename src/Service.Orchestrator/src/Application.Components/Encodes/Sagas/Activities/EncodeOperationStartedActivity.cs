using Giantnodes.Infrastructure.Uow.Services;
using Giantnodes.Service.Orchestrator.Domain.Aggregates.Encodes.Repositories;
using Giantnodes.Service.Orchestrator.Domain.Aggregates.Encodes.Values;
using Giantnodes.Service.Orchestrator.Persistence.Sagas;
using Giantnodes.Service.Encoder.Application.Contracts.Encoding.Events;
using MassTransit;

namespace Giantnodes.Service.Orchestrator.Application.Components.Encodes.Sagas.Activities;

public class EncodeOperationStartedActivity : IStateMachineActivity<EncodeSagaState, EncodeOperationStartedEvent>
{
    private readonly IUnitOfWorkService _uow;
    private readonly IEncodeRepository _repository;

    public EncodeOperationStartedActivity(IUnitOfWorkService uow, IEncodeRepository repository)
    {
        _uow = uow;
        _repository = repository;
    }

    public void Probe(ProbeContext context)
    {
        context.CreateScope(KebabCaseEndpointNameFormatter.Instance.Message<EncodeOperationStartedActivity>());
    }

    public void Accept(StateMachineVisitor visitor)
    {
        visitor.Visit(this);
    }

    public async Task Execute(
        BehaviorContext<EncodeSagaState, EncodeOperationStartedEvent> context,
        IBehavior<EncodeSagaState, EncodeOperationStartedEvent> next)
    {
        using var uow = await _uow.BeginAsync(context.CancellationToken);
        var encode = await _repository.SingleAsync(x => x.Id == context.Saga.EncodeId);

        encode.SetStarted(context.Message.RaisedAt);

        await uow.CommitAsync(context.CancellationToken);
        await next.Execute(context);
    }

    public Task Faulted<TException>(
        BehaviorExceptionContext<EncodeSagaState, EncodeOperationStartedEvent, TException> context,
        IBehavior<EncodeSagaState, EncodeOperationStartedEvent> next)
        where TException : Exception
    {
        return next.Faulted(context);
    }
}