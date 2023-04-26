using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Giantnodes.Service.Dashboard.Application;

public class Program
{
    public static async Task Main(string[] args)
    {
        var host = CreateHostBuilder(args).Build();
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