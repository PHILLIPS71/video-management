using Giantnodes.Infrastructure.Uow.Services;
using Giantnodes.Service.Orchestrator.Domain.Aggregates.Encodes.Repositories;
using Giantnodes.Service.Orchestrator.Domain.Aggregates.Encodes.Values;
using Giantnodes.Service.Orchestrator.Persistence.Sagas;
using Giantnodes.Service.Encoder.Application.Contracts.Encoding.Events;
using MassTransit;

namespace Giantnodes.Service.Orchestrator.Application.Components.Encodes.Sagas.Activities;

public class EncodeOperationOutputtedDataActivity : IStateMachineActivity<EncodeSagaState, EncodeOperationOutputtedEvent>
{
    private readonly IUnitOfWorkService _uow;
    private readonly IEncodeRepository _repository;

    public EncodeOperationOutputtedDataActivity(IUnitOfWorkService uow, IEncodeRepository repository)
    {
        _uow = uow;
        _repository = repository;
    }

    public void Probe(ProbeContext context)
    {
        context.CreateScope(KebabCaseEndpointNameFormatter.Instance.Message<EncodeOperationOutputtedDataActivity>());
    }

    public void Accept(StateMachineVisitor visitor)
    {
        visitor.Visit(this);
    }

    public async Task Execute(
        BehaviorContext<EncodeSagaState, EncodeOperationOutputtedEvent> context,
        IBehavior<EncodeSagaState, EncodeOperationOutputtedEvent> next)
    {
        using var uow = await _uow.BeginAsync(context.CancellationToken);
        var encode = await _repository.SingleAsync(x => x.Id == context.Saga.EncodeId);

        var speed = context.Message.Speed;
        if (speed.HasValue)
            encode.SetSpeed(new EncodeSpeed(speed.Value.Frames, speed.Value.Bitrate, speed.Value.Scale));

        encode.AppendOutputLog(context.Message.Output);

        await uow.CommitAsync(context.CancellationToken);
        await next.Execute(context);
    }

    public Task Faulted<TException>(
        BehaviorExceptionContext<EncodeSagaState, EncodeOperationOutputtedEvent, TException> context,
        IBehavior<EncodeSagaState, EncodeOperationOutputtedEvent> next)
        where TException : Exception
    {
        return next.Faulted(context);
    }
}