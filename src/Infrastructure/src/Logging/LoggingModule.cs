using Giantnodes.Infrastructure.Modules;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace Giantnodes.Infrastructure.Logging;

public static class LoggingModule
{
    /// <summary>
    /// Configure the Giantnodes logging module and its dependencies to the <paramref name="context" />.
    /// </summary>
    /// <param name="context"></param>
    /// <param name="configuration"></param>
    /// <returns></returns>
    public static IModuleContext UseLogging(this IModuleContext context, IConfiguration configuration)
    {
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .CreateLogger();

        return context;
    }
}