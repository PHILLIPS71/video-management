using Giantnodes.Infrastructure.Domain.Entities;
using Giantnodes.Infrastructure.Domain.Entities.Auditing;
using Giantnodes.Service.Dashboard.Application.Contracts.Files.Events;
using Giantnodes.Service.Dashboard.Domain.Aggregates.Entries.Files.Values;
using Giantnodes.Service.Dashboard.Domain.Shared.Enums;
using MassTransit;

namespace Giantnodes.Service.Dashboard.Domain.Aggregates.Entries.Files.Entities;

public class Transcode : AggregateRoot<Guid>, ITimestampableEntity
{
    public FileSystemFile File { get; private set; }

    public TranscodeStatus Status { get; private set; }

    public float? Percent { get; private set; }

    public TranscodeSpeed? Speed { get; private set; }

    public DateTime? StartedAt { get; private set; }

    public DateTime? FailedAt { get; private set; }

    public DateTime? DegradedAt { get; private set; }

    public DateTime? CancelledAt { get; private set; }

    public DateTime? CompletedAt { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public DateTime? UpdatedAt { get; private set; }

    private Transcode()
    {
    }

    internal Transcode(FileSystemFile file)
    {
        Id = NewId.NextSequentialGuid();
        File = file;
        Status = TranscodeStatus.Submitted;
    }

    public void SetStatus(TranscodeStatus status)
    {
        if (Status is TranscodeStatus.Completed or TranscodeStatus.Cancelled)
            throw new InvalidOperationException($"the status cannot be changed when in a {Status} status");

        switch (status)
        {
            case TranscodeStatus.Transcoding:
                StartedAt = DateTime.UtcNow;
                break;

            case TranscodeStatus.Cancelled:
                Status = TranscodeStatus.Cancelled;
                CancelledAt = DateTime.UtcNow;
                break;

            case TranscodeStatus.Degraded:
                DegradedAt = DateTime.UtcNow;
                break;

            case TranscodeStatus.Completed:
                CompletedAt = DateTime.UtcNow;
                break;
        }

        Status = status;
    }

    public void SetProgress(float progress)
    {
        if (Status is not TranscodeStatus.Transcoding)
            throw new InvalidOperationException($"the transcode is not in a {TranscodeStatus.Transcoding} status.");

        if (progress is < 0 or > 1)
            throw new ArgumentOutOfRangeException(nameof(progress), progress, "the progress percent value needs to be between 0 and 1.");

        Percent = progress;
    }

    public void SetSpeed(TranscodeSpeed speed)
    {
        if (Status is not TranscodeStatus.Transcoding)
            throw new InvalidOperationException($"the transcode is not in a {TranscodeStatus.Transcoding} status.");

        Speed = speed;
        DomainEvents.Add(new FileTranscodeSpeedChangedEvent
        {
            FileId = File.Id,
            TranscodeId = Id,
            Frames = Speed.Frames,
            Bitrate = Speed.Bitrate,
            Scale = Speed.Scale,
            RaisedAt = DateTime.UtcNow
        });
    }
}