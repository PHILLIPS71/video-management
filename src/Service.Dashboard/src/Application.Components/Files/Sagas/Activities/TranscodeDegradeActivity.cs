using Giantnodes.Infrastructure.Uow.Services;
using Giantnodes.Service.Dashboard.Domain.Aggregates.Entries.Files.Repositories;
using Giantnodes.Service.Dashboard.Domain.Shared.Enums;
using Giantnodes.Service.Dashboard.Persistence.Sagas;
using MassTransit;

namespace Giantnodes.Service.Dashboard.Application.Components.Files.Sagas.Activities;

public class TranscodeDegradeActivity : IStateMachineActivity<TranscodeSagaState>
{
    private readonly IUnitOfWorkService _uow;
    private readonly IFileSystemFileRepository _fileRepository;

    public TranscodeDegradeActivity(
        IUnitOfWorkService uow,
        IFileSystemFileRepository fileRepository)
    {
        _uow = uow;
        _fileRepository = fileRepository;
    }

    public void Probe(ProbeContext context)
    {
        context.CreateScope(KebabCaseEndpointNameFormatter.Instance.Message<TranscodeDegradeActivity>());
    }

    public void Accept(StateMachineVisitor visitor)
    {
        visitor.Visit(this);
    }

    public async Task Execute(BehaviorContext<TranscodeSagaState> context, IBehavior<TranscodeSagaState> next)
    {
        await Degrade(context);
        await next.Execute(context);
    }

    public async Task Execute<T>(BehaviorContext<TranscodeSagaState, T> context, IBehavior<TranscodeSagaState, T> next)
        where T : class
    {
        await Degrade(context);
        await next.Execute(context);
    }

    public Task Faulted<TException>(
        BehaviorExceptionContext<TranscodeSagaState, TException> context,
        IBehavior<TranscodeSagaState> next)
        where TException : Exception
    {
        return next.Faulted(context);
    }

    public Task Faulted<T, TException>(
        BehaviorExceptionContext<TranscodeSagaState, T, TException> context,
        IBehavior<TranscodeSagaState, T> next)
        where T : class where TException : Exception
    {
        return next.Faulted(context);
    }

    private async Task Degrade(ConsumeContext context)
    {
        using (var uow = await _uow.BeginAsync(context.CancellationToken))
        {
            var file = await _fileRepository.SingleAsync(x => x.Transcodes.Any(y => y.Id == context.CorrelationId));

            var transcode = file.Transcodes.First(x => x.Id == context.CorrelationId);
            transcode.SetStatus(TranscodeStatus.Degraded);

            await uow.CommitAsync(context.CancellationToken);
        }
    }
}