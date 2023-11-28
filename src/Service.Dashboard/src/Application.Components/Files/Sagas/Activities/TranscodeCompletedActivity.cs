using Giantnodes.Infrastructure.Uow.Services;
using Giantnodes.Service.Dashboard.Domain.Aggregates.Entries.Files.Repositories;
using Giantnodes.Service.Dashboard.Domain.Shared.Enums;
using Giantnodes.Service.Dashboard.Persistence.Sagas;
using MassTransit;
using MassTransit.Contracts.JobService;

namespace Giantnodes.Service.Dashboard.Application.Components.Files.Sagas.Activities;

public class TranscodeCompletedActivity : IStateMachineActivity<TranscodeSagaState, JobCompleted>
{
    private readonly IUnitOfWorkService _uow;
    private readonly IFileSystemFileRepository _fileRepository;

    public TranscodeCompletedActivity(
        IUnitOfWorkService uow,
        IFileSystemFileRepository fileRepository)
    {
        _uow = uow;
        _fileRepository = fileRepository;
    }

    public void Probe(ProbeContext context)
    {
        context.CreateScope(KebabCaseEndpointNameFormatter.Instance.Message<TranscodeCompletedActivity>());
    }

    public void Accept(StateMachineVisitor visitor)
    {
        visitor.Visit(this);
    }

    public async Task Execute(
        BehaviorContext<TranscodeSagaState, JobCompleted> context,
        IBehavior<TranscodeSagaState, JobCompleted> next)
    {
        using (var uow = await _uow.BeginAsync(context.CancellationToken))
        {
            var file = await _fileRepository.SingleAsync(x => x.Transcodes.Any(y => y.Id == context.CorrelationId));

            var transcode = file.Transcodes.First(x => x.Id == context.CorrelationId);
            transcode.SetStatus(TranscodeStatus.Completed);

            await uow.CommitAsync(context.CancellationToken);
        }

        await next.Execute(context);
    }

    public Task Faulted<TException>(
        BehaviorExceptionContext<TranscodeSagaState, JobCompleted, TException> context,
        IBehavior<TranscodeSagaState, JobCompleted> next) where TException : Exception
    {
        return next.Faulted(context);
    }
}