using Giantnodes.Infrastructure.Logging;
using Giantnodes.Infrastructure.Modules.Extensions;
using Giantnodes.Service.Encoder.Application.Components;
using Giantnodes.Service.Encoder.Persistence;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Xabe.FFmpeg;
using Xabe.FFmpeg.Downloader;

namespace Giantnodes.Service.Encoder.Console;

public static class Program
{
    public static async Task Main(string[] args)
    {
        var host = CreateHostBuilder(args).Build();

        FFmpeg.SetExecutablesPath(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "ffmpeg");
        await FFmpegDownloader.GetLatestVersion(FFmpegVersion.Official, FFmpeg.ExecutablesPath);

        await host.RunAsync();
    }

    private static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureServices((_, services) =>
            {
                var configuration = new ConfigurationBuilder()
                    .AddEnvironmentVariables()
                    .AddCommandLine(args)
                    .AddJsonFile("appsettings.json", false)
                    .AddJsonFile("appsettings.Development.json", true)
                    .Build();

                services
                    .AddGiantnodes(options => options.UseLogging(configuration));

                services
                    .AddPersistenceServices(configuration)
                    .AddApplicationServices()
                    .AddConsoleServices();
            });
}