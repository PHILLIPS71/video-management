using System.ComponentModel.DataAnnotations;

namespace Giantnodes.Service.Encoder.Application.Components.Settings;

public sealed class LimitSettings
{
    public const string ConfigurationSection = "Limit";

    /// <summary>
    /// Specifies the maximum number of file encoding jobs that can be processed concurrently.
    ///
    /// It should be carefully chosen based on the available system resources, setting a higher value may improve
    /// overall throughput, but it could also lead to resource contention and degraded performance or stability.
    /// </summary>
    [Required]
    public int MaxConcurrentEncodes { get; init; }
}