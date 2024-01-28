using Giantnodes.Infrastructure.Uow.Services;
using Giantnodes.Service.Dashboard.Domain.Aggregates.Encodes.Repositories;
using Giantnodes.Service.Dashboard.Domain.Aggregates.Entries.Files.Repositories;
using Giantnodes.Service.Dashboard.Persistence.Sagas;
using Giantnodes.Service.Encoder.Application.Contracts.Encoding.Events;
using MassTransit;

namespace Giantnodes.Service.Dashboard.Application.Components.Files.Sagas.Activities;

public class EncodeProgressedActivity : IStateMachineActivity<EncodeSagaState, EncodeProgressedEvent>
{
    private readonly IUnitOfWorkService _uow;
    private readonly IEncodeRepository _repository;

    public EncodeProgressedActivity(
        IUnitOfWorkService uow,
        IEncodeRepository repository)
    {
        _uow = uow;
        _repository = repository;
    }

    public void Probe(ProbeContext context)
    {
        context.CreateScope(KebabCaseEndpointNameFormatter.Instance.Message<EncodeProgressedActivity>());
    }

    public void Accept(StateMachineVisitor visitor)
    {
        visitor.Visit(this);
    }

    public async Task Execute(
        BehaviorContext<EncodeSagaState, EncodeProgressedEvent> context,
        IBehavior<EncodeSagaState, EncodeProgressedEvent> next)
    {
        using (var uow = await _uow.BeginAsync(context.CancellationToken))
        {
            var encode = await _repository.SingleAsync(x => x.Id == context.CorrelationId);
            encode.SetProgress(context.Message.Percent);

            await uow.CommitAsync(context.CancellationToken);
        }

        await next.Execute(context);
    }

    public Task Faulted<TException>(
        BehaviorExceptionContext<EncodeSagaState, EncodeProgressedEvent, TException> context,
        IBehavior<EncodeSagaState, EncodeProgressedEvent> next)
        where TException : Exception
    {
        return next.Faulted(context);
    }
}