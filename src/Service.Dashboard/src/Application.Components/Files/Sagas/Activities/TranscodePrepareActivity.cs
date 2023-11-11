using Giantnodes.Infrastructure.Uow.Services;
using Giantnodes.Service.Dashboard.Application.Contracts.Transcoding.Commands;
using Giantnodes.Service.Dashboard.Domain.Aggregates.Entries.Files.Repositories;
using Giantnodes.Service.Dashboard.Persistence.Sagas;
using MassTransit;

namespace Giantnodes.Service.Dashboard.Application.Components.Files.Sagas.Activities;

public class TranscodePrepareActivity : IStateMachineActivity<TranscodeSagaState, FileTranscode.Command>
{
    private readonly IUnitOfWorkService _uow;
    private readonly IFileSystemFileRepository _fileRepository;

    public TranscodePrepareActivity(
        IUnitOfWorkService uow,
        IFileSystemFileRepository fileRepository)
    {
        _uow = uow;
        _fileRepository = fileRepository;
    }

    public void Probe(ProbeContext context)
    {
        context.CreateScope(KebabCaseEndpointNameFormatter.Instance.Message<TranscodePrepareActivity>());
    }

    public void Accept(StateMachineVisitor visitor)
    {
        visitor.Visit(this);
    }

    public async Task Execute(
        BehaviorContext<TranscodeSagaState, FileTranscode.Command> context,
        IBehavior<TranscodeSagaState, FileTranscode.Command> next)
    {
        using (var uow = _uow.Begin())
        {
            var file = await _fileRepository.SingleAsync(x => x.Id == context.Message.FileId);

            var transcode = file.Transcode();
            context.Saga.InputFullPath = file.PathInfo.FullName;

            await uow.CommitAsync(context.CancellationToken);

            context.Saga.CorrelationId = transcode.Id;
        }

        await next.Execute(context);
    }

    public Task Faulted<TException>(
        BehaviorExceptionContext<TranscodeSagaState, FileTranscode.Command, TException> context,
        IBehavior<TranscodeSagaState, FileTranscode.Command> next)
        where TException : Exception
    {
        return next.Faulted(context);
    }
}