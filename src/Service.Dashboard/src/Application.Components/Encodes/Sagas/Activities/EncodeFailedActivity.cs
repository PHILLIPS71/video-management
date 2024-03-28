using Giantnodes.Infrastructure.Uow.Services;
using Giantnodes.Service.Dashboard.Domain.Aggregates.Encodes.Repositories;
using Giantnodes.Service.Dashboard.Persistence.Sagas;
using MassTransit;

namespace Giantnodes.Service.Dashboard.Application.Components.Encodes.Sagas.Activities;

public class EncodeFailedActivity : IStateMachineActivity<EncodeSagaState>
{
    private readonly IUnitOfWorkService _uow;
    private readonly IEncodeRepository _repository;

    public EncodeFailedActivity(IUnitOfWorkService uow, IEncodeRepository repository)
    {
        _uow = uow;
        _repository = repository;
    }

    public void Probe(ProbeContext context)
    {
        context.CreateScope(KebabCaseEndpointNameFormatter.Instance.Message<EncodeFailedActivity>());
    }

    public void Accept(StateMachineVisitor visitor)
    {
        visitor.Visit(this);
    }

    public async Task Execute(BehaviorContext<EncodeSagaState> context, IBehavior<EncodeSagaState> next)
    {
        await Execute(context);
        await next.Execute(context);
    }

    public async Task Execute<T>(BehaviorContext<EncodeSagaState, T> context, IBehavior<EncodeSagaState, T> next)
        where T : class
    {
        await Execute(context);
        await next.Execute(context);
    }

    public async Task Faulted<TException>(
        BehaviorExceptionContext<EncodeSagaState, TException> context,
        IBehavior<EncodeSagaState> next)
        where TException : Exception
    {
        await Execute(context);
        await next.Faulted(context);
    }

    public async Task Faulted<T, TException>(
        BehaviorExceptionContext<EncodeSagaState, T, TException> context,
        IBehavior<EncodeSagaState, T> next)
        where T : class
        where TException : Exception
    {
        await Execute(context);
        await next.Faulted(context);
    }

    private async Task Execute(SagaConsumeContext<EncodeSagaState> context)
    {
        using var uow = await _uow.BeginAsync(context.CancellationToken);

        var encode = await _repository.SingleAsync(x => x.Id == context.Saga.EncodeId);
        encode.SetFailed(InVar.Timestamp, context.Saga.FailedReason ?? "unknown");

        await uow.CommitAsync(context.CancellationToken);
    }
}