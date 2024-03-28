using Ardalis.GuardClauses;
using Giantnodes.Infrastructure.Domain.Entities;
using Giantnodes.Infrastructure.Domain.Entities.Auditing;
using Giantnodes.Service.Dashboard.Application.Contracts.Encodes.Events;
using Giantnodes.Service.Dashboard.Domain.Aggregates.EncodeProfiles;
using Giantnodes.Service.Dashboard.Domain.Aggregates.Encodes.Entities;
using Giantnodes.Service.Dashboard.Domain.Aggregates.Encodes.Values;
using Giantnodes.Service.Dashboard.Domain.Aggregates.Entries.Files;
using Giantnodes.Service.Dashboard.Domain.Shared.Enums;
using Giantnodes.Service.Dashboard.Domain.Values;
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

    public string? FfmpegCommand { get; private set; }

    public Machine? Machine { get; private set; }

    public DateTime? StartedAt { get; private set; }

    public DateTime? FailedAt { get; private set; }

    public string? FailureReason { get; private set; }

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

    public void SetStarted(DateTime when)
    {
        Guard.Against.FutureDate(when);

        Status = EncodeStatus.Encoding;
        StartedAt = when;
    }

    public void SetCompleted(DateTime when)
    {
        Guard.Against.FutureDate(when);

        if (Status == EncodeStatus.Cancelled)
            throw new InvalidOperationException("cannot set a encode to completed that has been cancelled.");

        Status = EncodeStatus.Completed;
        CompletedAt = DateTime.UtcNow;
        Percent = 1.0f;
    }

    public void SetFailed(DateTime when, string reason)
    {
        Guard.Against.FutureDate(when);
        Guard.Against.NullOrWhiteSpace(reason);

        Status = EncodeStatus.Failed;
        FailureReason = reason;
        FailedAt = when;
    }

    public void SetCancelled(DateTime when)
    {
        Guard.Against.FutureDate(when);

        if (Status == EncodeStatus.Cancelled)
            return;

        Status = EncodeStatus.Cancelled;
        CancelledAt = DateTime.UtcNow;

        DomainEvents.Add(new EncodeCancelledEvent { EncodeId = Id });
    }

    public void SetProgress(float progress)
    {
        if (Status is not EncodeStatus.Encoding)
            throw new InvalidOperationException("cannot set progress when not in a encoding status.");

        Guard.Against.OutOfRange(progress, nameof(progress), 0, 1);

        Percent = progress;
    }

    public void SetSpeed(EncodeSpeed speed)
    {
        if (Status is not EncodeStatus.Encoding)
            throw new InvalidOperationException("cannot set speed when not in a encoding status.");

        Speed = speed;

        DomainEvents.Add(new EncodeSpeedChangedEvent
        {
            EncodeId = Id,
            Frames = Speed.Frames,
            Bitrate = Speed.Bitrate,
            Scale = Speed.Scale,
        });
    }

    public void SetFfmpegConversion(Machine machine, string command)
    {
        Guard.Against.NullOrWhiteSpace(command);

        Machine = machine;
        FfmpegCommand = command;
    }

    public void AddSnapshot(EncodeSnapshot snapshot)
    {
        _snapshots.Add(snapshot);
    }
}