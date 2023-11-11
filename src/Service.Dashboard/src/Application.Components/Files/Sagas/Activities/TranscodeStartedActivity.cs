using Giantnodes.Infrastructure.Uow.Services;
using Giantnodes.Service.Dashboard.Domain.Aggregates.Entries.Files.Repositories;
using Giantnodes.Service.Dashboard.Domain.Shared.Enums;
using Giantnodes.Service.Dashboard.Persistence.Sagas;
using MassTransit;
using MassTransit.Contracts.JobService;

namespace Giantnodes.Service.Dashboard.Application.Components.Files.Sagas.Activities;

public class TranscodeStartedActivity : IStateMachineActivity<TranscodeSagaState, JobStarted>
{
    private readonly IUnitOfWorkService _uow;
    private readonly IFileSystemFileRepository _fileRepository;

    public TranscodeStartedActivity(
        IUnitOfWorkService uow,
        IFileSystemFileRepository fileRepository)
    {
        _uow = uow;
        _fileRepository = fileRepository;
    }

    public void Probe(ProbeContext context)
    {
        context.CreateScope(KebabCaseEndpointNameFormatter.Instance.Message<TranscodeStartedActivity>());
    }

    public void Accept(StateMachineVisitor visitor)
    {
        visitor.Visit(this);
    }

    public async Task Execute(
        BehaviorContext<TranscodeSagaState, JobStarted> context,
        IBehavior<TranscodeSagaState, JobStarted> next)
    {
        using (var uow = _uow.Begin())
        {
            var file = await _fileRepository.SingleAsync(x => x.Transcodes.Any(y => y.Id == context.CorrelationId));

            var transcode = file.Transcodes.First(x => x.Id == context.CorrelationId);
            transcode.SetStatus(TranscodeStatus.Transcoding);

            await uow.CommitAsync(context.CancellationToken);
        }

        await next.Execute(context);
    }

    public Task Faulted<TException>(
        BehaviorExceptionContext<TranscodeSagaState, JobStarted, TException> context,
        IBehavior<TranscodeSagaState, JobStarted> next)
        where TException : Exception
    {
        return next.Faulted(context);
    }
}