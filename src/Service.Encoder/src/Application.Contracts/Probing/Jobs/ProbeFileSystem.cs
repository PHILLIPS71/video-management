namespace Giantnodes.Service.Encoder.Application.Contracts.Probing.Jobs;

public sealed record ProbeFileSystem
{
    public required string FullPath { get; init; }
}