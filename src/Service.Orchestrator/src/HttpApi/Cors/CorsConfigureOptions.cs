using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.Extensions.Options;

namespace Giantnodes.Service.Orchestrator.HttpApi.Cors;

public class CorsConfigureOptions : IConfigureNamedOptions<CorsOptions>
{
    private const string ConfigurationSectionKey = "CorsOrigins";

    private readonly IConfiguration _configuration;

    public CorsConfigureOptions(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void Configure(CorsOptions options)
    {
        var origins = _configuration.GetValue<string>(ConfigurationSectionKey);
        if (string.IsNullOrWhiteSpace(origins))
            throw new InvalidOperationException($"The configuration section '{ConfigurationSectionKey}' cannot be null or empty.");

        options
            .AddDefaultPolicy(builder =>
            {
                builder
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .WithOrigins(origins.Split(","));
            });
    }

    public void Configure(string? name, CorsOptions options)
    {
        Configure(options);
    }
}