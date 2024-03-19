using Giantnodes.Infrastructure.Uow.Services;
using Giantnodes.Service.Dashboard.Domain.Aggregates.Encodes.Repositories;
using Giantnodes.Service.Dashboard.Persistence.Sagas;
using Giantnodes.Service.Encoder.Application.Contracts.Encoding.Commands;
using MassTransit;

namespace Giantnodes.Service.Dashboard.Application.Components.Encodes.Sagas.Activities;

public class EncodeOperationRequestActivity : IStateMachineActivity<EncodeSagaState>
{
    private readonly IUnitOfWorkService _uow;
    private readonly IEncodeRepository _repository;

    public EncodeOperationRequestActivity(IUnitOfWorkService uow, IEncodeRepository repository)
    {
        _uow = uow;
        _repository = repository;
    }

    public void Probe(ProbeContext context)
    {
        context.CreateScope(KebabCaseEndpointNameFormatter.Instance.Message<EncodeOperationRequestActivity>());
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
        where T : class
        where TException : Exception
    {
        return next.Faulted(context);
    }

    private async Task Execute(SagaConsumeContext<EncodeSagaState> context)
    {
        using (await _uow.BeginAsync(context.CancellationToken))
        {
            var encode = await _repository.SingleAsync(x => x.Id == context.Saga.EncodeId);

            await context.Publish(new EncodeOperationSubmit.Command
            {
                CorrelationId = context.Saga.CorrelationId,
                InputFilePath = context.Saga.InputFilePath,
                OutputFilePath = context.Saga.OutputFilePath,
                Codec = encode.Profile.Codec.Value,
                Preset = encode.Profile.Preset.Value,
                Tune = encode.Profile.Tune?.Value,
                Quality = encode.Profile.Quality,
                UseHardwareAcceleration = encode.Profile.UseHardwareAcceleration
            });
        }
    }
}