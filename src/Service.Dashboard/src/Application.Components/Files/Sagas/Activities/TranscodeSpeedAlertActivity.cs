using Giantnodes.Infrastructure.Uow.Services;
using Giantnodes.Service.Dashboard.Domain.Aggregates.Entries.Files.Repositories;
using Giantnodes.Service.Dashboard.Domain.Aggregates.Entries.Files.Values;
using Giantnodes.Service.Dashboard.Persistence.Sagas;
using Giantnodes.Service.Encoder.Application.Contracts.Encoding.Events;
using MassTransit;

namespace Giantnodes.Service.Dashboard.Application.Components.Files.Sagas.Activities;

public class TranscodeSpeedAlertActivity : IStateMachineActivity<TranscodeSagaState, TranscodeSpeedAlertEvent>
{
    private readonly IUnitOfWorkService _uow;
    private readonly IFileSystemFileRepository _fileRepository;

    public TranscodeSpeedAlertActivity(
        IUnitOfWorkService uow,
        IFileSystemFileRepository fileRepository)
    {
        _uow = uow;
        _fileRepository = fileRepository;
    }

    public void Probe(ProbeContext context)
    {
        context.CreateScope(KebabCaseEndpointNameFormatter.Instance.Message<TranscodeSpeedAlertActivity>());
    }

    public void Accept(StateMachineVisitor visitor)
    {
        visitor.Visit(this);
    }

    public async Task Execute(
        BehaviorContext<TranscodeSagaState, TranscodeSpeedAlertEvent> context,
        IBehavior<TranscodeSagaState, TranscodeSpeedAlertEvent> next)
    {
        using (var uow = _uow.Begin())
        {
            var file = await _fileRepository.SingleAsync(x => x.Transcodes.Any(y => y.Id == context.CorrelationId));

            var transcode = file.Transcodes.First(x => x.Id == context.CorrelationId);

            var speed = new TranscodeSpeed(
                context.Message.Frames,
                context.Message.Bitrate,
                context.Message.Scale);

            transcode.SetSpeed(speed);
            await uow.CommitAsync(context.CancellationToken);
        }

        await next.Execute(context);
    }

    public Task Faulted<TException>(
        BehaviorExceptionContext<TranscodeSagaState, TranscodeSpeedAlertEvent, TException> context,
        IBehavior<TranscodeSagaState, TranscodeSpeedAlertEvent> next)
        where TException : Exception
    {
        return next.Faulted(context);
    }
}