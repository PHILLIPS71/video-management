using Serilog;

namespace Giantnodes.Service.Dashboard.HttpApi;

public static class Program
{
    public static async Task Main(string[] args)
    {
        var host = CreateHostBuilder(args).Build();
        await host.RunAsync();
    }

    private static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(builder => builder.UseStartup<Startup>())
            .UseSerilog((context, configuration) => configuration.ReadFrom.Configuration(context.Configuration));
}