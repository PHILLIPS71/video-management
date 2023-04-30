using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Xabe.FFmpeg;
using Xabe.FFmpeg.Downloader;

namespace Giantnodes.Service.Encoder.Application;

public class Program
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
            .ConfigureServices((builder, services) =>
            {
                var configuration = new ConfigurationBuilder()
                    .AddEnvironmentVariables()
                    .AddCommandLine(args)
                    .AddJsonFile("appsettings.Development.json")
                    .Build();

                services
                    .AddApplicationServices(configuration);
            });
}