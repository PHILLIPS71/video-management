using Giantnodes.Infrastructure.Uow.Services;
using Giantnodes.Service.Dashboard.Domain.Aggregates.Encodes.Repositories;
using Giantnodes.Service.Dashboard.Domain.Aggregates.Encodes.Values;
using Giantnodes.Service.Dashboard.Persistence.Sagas;
using Giantnodes.Service.Encoder.Application.Contracts.Encoding.Events;
using MassTransit;

namespace Giantnodes.Service.Dashboard.Application.Components.Encodes.Sagas.Activities;

public class EncodeHeartbeatActivity : IStateMachineActivity<EncodeSagaState, EncodeHeartbeatEvent>
{
    private readonly IUnitOfWorkService _uow;
    private readonly IEncodeRepository _repository;

    public EncodeHeartbeatActivity(
        IUnitOfWorkService uow,
        IEncodeRepository repository)
    {
        _uow = uow;
        _repository = repository;
    }

    public void Probe(ProbeContext context)
    {
        context.CreateScope(KebabCaseEndpointNameFormatter.Instance.Message<EncodeHeartbeatActivity>());
    }

    public void Accept(StateMachineVisitor visitor)
    {
        visitor.Visit(this);
    }

    public async Task Execute(
        BehaviorContext<EncodeSagaState, EncodeHeartbeatEvent> context,
        IBehavior<EncodeSagaState, EncodeHeartbeatEvent> next)
    {
        using (var uow = await _uow.BeginAsync(context.CancellationToken))
        {
            var encode = await _repository.SingleAsync(x => x.Id == context.CorrelationId);

            var speed = new EncodeSpeed(context.Message.Frames, context.Message.Bitrate, context.Message.Scale);
            encode.SetSpeed(speed);

            await uow.CommitAsync(context.CancellationToken);
        }

        await next.Execute(context);
    }

    public Task Faulted<TException>(
        BehaviorExceptionContext<EncodeSagaState, EncodeHeartbeatEvent, TException> context,
        IBehavior<EncodeSagaState, EncodeHeartbeatEvent> next)
        where TException : Exception
    {
        return next.Faulted(context);
    }
}