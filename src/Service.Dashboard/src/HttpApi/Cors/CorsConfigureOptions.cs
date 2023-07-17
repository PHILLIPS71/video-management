using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.Extensions.Options;

namespace Giantnodes.Service.Dashboard.HttpApi.Cors;

public class CorsConfigureOptions : IConfigureNamedOptions<CorsOptions>
{
    private const string ConfigurationSectionKey = "AllowedOrigins";

    private readonly IConfiguration _configuration;

    public CorsConfigureOptions(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void Configure(CorsOptions options)
    {
        var origins = _configuration.GetValue<string>(ConfigurationSectionKey);
        if (string.IsNullOrWhiteSpace(origins))
            throw new InvalidOperationException($"The connection string '{ConfigurationSectionKey}' cannot be null or empty.");

        options.AddDefaultPolicy(builder =>
        {
            builder
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials()
                .WithOrigins(origins.Split(","));
        });
    }

    public void Configure(string? name, CorsOptions options)
    {
        Configure(options);
    }
}