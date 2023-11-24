namespace Giantnodes.Service.Encoder.Application.Contracts.Encoding.Events;

public sealed record TranscodeSpeedAlertEvent
{
    public required Guid JobId { get; init; }

    public required float Frames { get; set; }

    public required long Bitrate { get; set; }

    public required float Scale { get; set; }
}