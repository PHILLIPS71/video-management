using Giantnodes.Infrastructure.Domain.Entities;
using Giantnodes.Infrastructure.Domain.Entities.Auditing;
using Giantnodes.Service.Dashboard.Application.Contracts.Encodes.Events;
using Giantnodes.Service.Dashboard.Domain.Aggregates.EncodeProfiles;
using Giantnodes.Service.Dashboard.Domain.Aggregates.Encodes.Entities;
using Giantnodes.Service.Dashboard.Domain.Aggregates.Encodes.Values;
using Giantnodes.Service.Dashboard.Domain.Aggregates.Entries.Files;
using Giantnodes.Service.Dashboard.Domain.Shared.Enums;
using MassTransit;

namespace Giantnodes.Service.Dashboard.Domain.Aggregates.Encodes;

public class Encode : AggregateRoot<Guid>, ITimestampableEntity
{
    private readonly List<EncodeSnapshot> _snapshots = new();

    public FileSystemFile File { get; private set; }

    public EncodeProfile Profile { get; private set; }

    public EncodeSpeed? Speed { get; private set; }

    public EncodeStatus Status { get; private set; }

    public float? Percent { get; private set; }

    public DateTime? StartedAt { get; private set; }

    public DateTime? FailedAt { get; private set; }

    public DateTime? DegradedAt { get; private set; }

    public DateTime? CancelledAt { get; private set; }

    public DateTime? CompletedAt { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public DateTime? UpdatedAt { get; private set; }

    public IReadOnlyCollection<EncodeSnapshot> Snapshots { get; private set; }

    private Encode()
    {
        Snapshots = _snapshots;
    }

    public Encode(FileSystemFile file, EncodeProfile profile)
    {
        Id = NewId.NextSequentialGuid();
        File = file;
        Profile = profile;
        Status = EncodeStatus.Submitted;
        Snapshots = _snapshots;

        DomainEvents.Add(new EncodeCreatedEvent
        {
            EncodeId = Id,
            EncodeProfileId = Profile.Id,
            FileId = File.Id,
            FilePath = File.PathInfo.FullName
        });
    }

    public void SetStatus(EncodeStatus status)
    {
        if (Status is EncodeStatus.Completed or EncodeStatus.Cancelled)
            throw new InvalidOperationException($"the status cannot be changed when in a {Status} status");

        switch (status)
        {
            case EncodeStatus.Encoding:
                StartedAt = DateTime.UtcNow;
                break;

            case EncodeStatus.Cancelled:
                Status = EncodeStatus.Cancelled;
                CancelledAt = DateTime.UtcNow;

                DomainEvents.Add(new EncodeCancelledEvent { EncodeId = Id });
                break;

            case EncodeStatus.Degraded:
                DegradedAt = DateTime.UtcNow;
                break;

            case EncodeStatus.Failed:
                FailedAt = DateTime.UtcNow;
                break;

            case EncodeStatus.Completed:
                Percent = 1.0f;
                CompletedAt = DateTime.UtcNow;
                break;
        }

        Status = status;
    }

    public void SetProgress(float progress)
    {
        if (Status is not EncodeStatus.Encoding)
            throw new InvalidOperationException($"the encode is not in a {EncodeStatus.Encoding} status.");

        if (progress is < 0 or > 1)
            throw new ArgumentOutOfRangeException(nameof(progress), progress, "the progress percent value needs to be between 0 and 1.");

        Percent = progress;
    }

    public void SetSpeed(EncodeSpeed speed)
    {
        if (Status is not EncodeStatus.Encoding)
            throw new InvalidOperationException($"the encode is not in a {EncodeStatus.Encoding} status.");

        Speed = speed;

        DomainEvents.Add(new EncodeSpeedChangedEvent
        {
            EncodeId = Id,
            Frames = Speed.Frames,
            Bitrate = Speed.Bitrate,
            Scale = Speed.Scale,
        });
    }

    public void AddSnapshot(EncodeSnapshot snapshot)
    {
        _snapshots.Add(snapshot);
    }
}