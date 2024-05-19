using Ardalis.GuardClauses;
using Giantnodes.Infrastructure.Domain.Entities;
using Giantnodes.Infrastructure.Domain.Entities.Auditing;
using Giantnodes.Service.Dashboard.Application.Contracts.Encodes.Events;
using Giantnodes.Service.Dashboard.Domain.Aggregates.Encodes.Entities;
using Giantnodes.Service.Dashboard.Domain.Aggregates.Encodes.Values;
using Giantnodes.Service.Dashboard.Domain.Aggregates.Entries.Files;
using Giantnodes.Service.Dashboard.Domain.Aggregates.Recipes;
using Giantnodes.Service.Dashboard.Domain.Shared.Enums;
using Giantnodes.Service.Dashboard.Domain.Values;
using MassTransit;

namespace Giantnodes.Service.Dashboard.Domain.Aggregates.Encodes;

public class Encode : AggregateRoot<Guid>, ITimestampableEntity
{
    private readonly List<EncodeSnapshot> _snapshots = new();

    private EncodeStatus _status;

    /// <summary>
    /// The file being encoded.
    /// </summary>
    public FileSystemFile File { get; private set; }

    /// <summary>
    /// The recipe used for encoding.
    /// </summary>
    public Recipe Recipe { get; private set; }

    /// <summary>
    /// The current encoding speed.
    /// </summary>
    public EncodeSpeed? Speed { get; private set; }

    /// <summary>
    /// The current status of the encoding process.
    /// </summary>
    public EncodeStatus Status
    {
        get => _status;
        private set
        {
            if (Status != value)
                DomainEvents.Add(new EncodeStatusChangedEvent { EncodeId = Id, FromStatus = Status, ToStatus = value });

            _status = value;
        }
    }

    /// <summary>
    /// The machine performing the encoding.
    /// </summary>
    public Machine? Machine { get; private set; }

    /// <summary>
    /// The current progress percentage of the encoding process.
    /// </summary>
    public float? Percent { get; private set; }

    /// <summary>
    /// The ffmpeg command used for the encoding.
    /// </summary>
    public string? Command { get; private set; }

    /// <summary>
    /// The ffmpeg output log of the encoding process.
    /// </summary>
    public string? Output { get; private set; }

    /// <summary>
    /// The timestamp when the encoding started.
    /// </summary>
    public DateTime? StartedAt { get; private set; }

    /// <summary>
    /// The timestamp when the encoding failed.
    /// </summary>
    public DateTime? FailedAt { get; private set; }

    /// <summary>
    /// The reason for the encoding failure.
    /// </summary>
    public string? FailureReason { get; private set; }

    /// <summary>
    /// The timestamp when the encoding was degraded.
    /// </summary>
    public DateTime? DegradedAt { get; private set; }

    /// <summary>
    /// The timestamp when the encoding was cancelled.
    /// </summary>
    public DateTime? CancelledAt { get; private set; }

    /// <summary>
    /// The timestamp when the encoding was completed.
    /// </summary>
    public DateTime? CompletedAt { get; private set; }

    /// <inheritdoc />
    public DateTime CreatedAt { get; private set; }

    /// <inheritdoc />
    public DateTime? UpdatedAt { get; private set; }

    /// <summary>
    /// A collection of snapshots taken during the encoding process.
    /// </summary>
    public IReadOnlyCollection<EncodeSnapshot> Snapshots { get; private set; }

    private Encode()
    {
        Snapshots = _snapshots;
    }

    public Encode(FileSystemFile file, Recipe recipe)
    {
        Id = NewId.NextSequentialGuid();
        File = file;
        Recipe = recipe;
        Status = EncodeStatus.Submitted;
        Snapshots = _snapshots;

        DomainEvents.Add(new EncodeCreatedEvent
        {
            EncodeId = Id,
            RecipeId = Recipe.Id,
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

    /// <summary>
    /// Sets the progress of the encoding process.
    /// </summary>
    /// <param name="progress">The current progress percentage.</param>
    public void SetProgress(float progress)
    {
        if (Status is not EncodeStatus.Encoding)
            throw new InvalidOperationException("cannot set progress when not in a encoding status.");

        Guard.Against.OutOfRange(progress, nameof(progress), 0, 1);

        Percent = progress;

        DomainEvents.Add(new EncodeProgressedEvent
        {
            EncodeId = Id,
            Percent = Percent.Value
        });
    }

    /// <summary>
    /// Sets the encoding speed.
    /// </summary>
    /// <param name="speed">The encoding speed.</param>
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

    /// <summary>
    /// Sets the ffmpeg conversion command and the machine performing the encoding.
    /// </summary>
    /// <param name="machine">The machine performing the encoding.</param>
    /// <param name="command">The ffmpeg command for the encoding.</param>
    public void SetFfmpegConversion(Machine machine, string command)
    {
        Guard.Against.NullOrWhiteSpace(command);

        Machine = machine;
        Command = command;
    }

    /// <summary>
    /// Appends the given ffmpeg output to the encoding log.
    /// </summary>
    /// <param name="output">The ffmpeg output to be appended.</param>
    public void AppendOutputLog(string output)
    {
        Guard.Against.NullOrWhiteSpace(output);

        Output = string.Join(Environment.NewLine, Output, output);

        DomainEvents.Add(new EncodeOutputtedEvent
        {
            EncodeId = Id,
            Output = output,
            FullOutput = Output
        });
    }

    /// <summary>
    /// Adds a snapshot taken during the encoding process.
    /// </summary>
    /// <param name="snapshot">The snapshot to be added.</param>
    public void AddSnapshot(EncodeSnapshot snapshot)
    {
        _snapshots.Add(snapshot);
    }
}