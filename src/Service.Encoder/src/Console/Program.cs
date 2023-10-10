﻿using Giantnodes.Service.Encoder.Application.Components;
using Giantnodes.Service.Encoder.Persistence;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
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
            .ConfigureServices((context, services) =>
            {
                var configuration = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .AddJsonFile($"appsettings.{context.HostingEnvironment.EnvironmentName}.json", optional: true, reloadOnChange: true)
                    .AddEnvironmentVariables()
                    .AddCommandLine(args)
                    .Build();
                
                services
                    .AddPersistenceServices(configuration)
                    .AddApplicationServices()
                    .AddConsoleServices();
            })
            .UseSerilog((context, configuration) => configuration.ReadFrom.Configuration(context.Configuration));
}