using Giantnodes.Service.Dashboard.Application.Contracts.Encoding.Events;
using Giantnodes.Service.Dashboard.Domain.Shared.Enums;
using Giantnodes.Service.Dashboard.Persistence.DbContexts;
using Giantnodes.Service.Dashboard.Persistence.Sagas;
using Giantnodes.Service.Encoder.Application.Contracts.Encoding.Jobs;
using MassTransit;
using MassTransit.Contracts.JobService;
using Microsoft.EntityFrameworkCore;

namespace Giantnodes.Service.Dashboard.Application.Encoding.StateMachines.Activities;

public class EncodePrepareActivity : IStateMachineActivity<EncodeSaga, EncodeSubmittedEvent>
{
    private readonly ApplicationDbContext _database;
    private readonly IRequestClient<EncodeFile> _request;

    public EncodePrepareActivity(ApplicationDbContext database, IRequestClient<EncodeFile> request)
    {
        _database = database;
        _request = request;
    }

    public void Probe(ProbeContext context)
    {
        context.CreateScope(KebabCaseEndpointNameFormatter.Instance.Message<EncodePrepareActivity>());
    }

    public void Accept(StateMachineVisitor visitor)
    {
        visitor.Visit(this);
    }

    public async Task Execute(BehaviorContext<EncodeSaga, EncodeSubmittedEvent> context, IBehavior<EncodeSaga, EncodeSubmittedEvent> next)
    {
        var encode = await _database
            .Encodes
            .Include(x => x.Preset)
                .ThenInclude(x => x!.VideoStreams)
            .Include(x => x.Preset)
                .ThenInclude(x => x!.AudioStreams)
            .Include(x => x.Preset)
                .ThenInclude(x => x!.SubtitleStreams)
            .SingleAsync(x => x.Id == context.Saga.CorrelationId, context.CancellationToken);

        var preset = encode.Preset;
        if (preset == null)
            throw new InvalidOperationException(nameof(preset));
        
        if (preset.VideoStreams == null)
            throw new InvalidOperationException(nameof(preset));

        var request = new EncodeFile
        {
            FullPath = context.Saga.FullPath,
            Preset = preset.EncodePreset,
            VideoStreams = preset
                .VideoStreams
                .Select(x => new EncodeFileVideoStream
                {
                    Codec = x.Codec,
                    Height = x.Height,
                    Width = x.Width,
                    Bitrate = x.Bitrate,
                    Framerate = x.Framerate
                })
                .ToArray(),
            AudioStreams = preset
                .AudioStreams?
                .Select(x => new EncodeFileAudioStream
                {
                    Codec = x.Codec,
                    Channels = x.Channels,
                    Bitrate = x.Bitrate,
                })
                .ToArray() ?? Array.Empty<EncodeFileAudioStream>()
        };
        
        var response = await _request.GetResponse<JobSubmissionAccepted>(request);
        context.Saga.JobId = response.Message.JobId;

        encode.Status = EncodeStatus.Submitted;
        await _database.SaveChangesAsync(context.CancellationToken);
        await next.Execute(context);
    }

    public Task Faulted<TException>(BehaviorExceptionContext<EncodeSaga, EncodeSubmittedEvent, TException> context,
        IBehavior<EncodeSaga, EncodeSubmittedEvent> next)
        where TException : Exception
    {
        return next.Faulted(context);
    }
}