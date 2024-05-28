using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace Giantnodes.Service.Dashboard.Persistence.HostedServices;

public sealed class MigratorHostedService<TDbContext> : IHostedService
    where TDbContext : DbContext
{
    private readonly IServiceScopeFactory _factory;

    public MigratorHostedService(IServiceScopeFactory factory)
    {
        _factory = factory;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        var stopwatch = new Stopwatch();
        using var scope = _factory.CreateScope();

        var database = scope.ServiceProvider.GetRequiredService<TDbContext>();
        var pending = await database.Database.GetPendingMigrationsAsync(cancellationToken);
        var total = pending.Count();

        if (total <= 0)
        {
            Log.Information("no pending migrations for database context {0}.", typeof(TDbContext).Name);
            return;
        }

        Log.Information("applying {0} pending migrations for database context {1}...", total, typeof(TDbContext).Name);
        stopwatch.Start();
        await database.Database.MigrateAsync(cancellationToken);
        stopwatch.Stop();
        Log.Information("successfully applied {0} migrations for database context {1} in {2} ms.", total, typeof(TDbContext).Name, stopwatch.ElapsedMilliseconds);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}