using Giantnodes.Infrastructure.Uow.Services;
using Giantnodes.Service.Dashboard.Domain.Aggregates.Encodes.Repositories;
using Giantnodes.Service.Dashboard.Domain.Aggregates.Entries.Files.Repositories;
using Giantnodes.Service.Dashboard.Domain.Shared.Enums;
using Giantnodes.Service.Dashboard.Persistence.Sagas;
using MassTransit;

namespace Giantnodes.Service.Dashboard.Application.Components.Files.Sagas.Activities;

public class EncodeDegradeActivity : IStateMachineActivity<EncodeSagaState>
{
    private readonly IUnitOfWorkService _uow;
    private readonly IEncodeRepository _repository;

    public EncodeDegradeActivity(
        IUnitOfWorkService uow,
        IEncodeRepository repository)
    {
        _uow = uow;
        _repository = repository;
    }

    public void Probe(ProbeContext context)
    {
        context.CreateScope(KebabCaseEndpointNameFormatter.Instance.Message<EncodeDegradeActivity>());
    }

    public void Accept(StateMachineVisitor visitor)
    {
        visitor.Visit(this);
    }

    public async Task Execute(BehaviorContext<EncodeSagaState> context, IBehavior<EncodeSagaState> next)
    {
        await Degrade(context);
        await next.Execute(context);
    }

    public async Task Execute<T>(BehaviorContext<EncodeSagaState, T> context, IBehavior<EncodeSagaState, T> next)
        where T : class
    {
        await Degrade(context);
        await next.Execute(context);
    }

    public Task Faulted<TException>(
        BehaviorExceptionContext<EncodeSagaState, TException> context,
        IBehavior<EncodeSagaState> next)
        where TException : Exception
    {
        return next.Faulted(context);
    }

    public Task Faulted<T, TException>(
        BehaviorExceptionContext<EncodeSagaState, T, TException> context,
        IBehavior<EncodeSagaState, T> next)
        where T : class where TException : Exception
    {
        return next.Faulted(context);
    }

    private async Task Degrade(ConsumeContext context)
    {
        using (var uow = await _uow.BeginAsync(context.CancellationToken))
        {
            var encode = await _repository.SingleAsync(x => x.Id == context.CorrelationId);

            encode.SetStatus(EncodeStatus.Degraded);
            await uow.CommitAsync(context.CancellationToken);
        }
    }
}