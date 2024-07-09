using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Microsoft.EntityFrameworkCore;

internal class AutoMigrateDbContextAfterAppStarted<TDbContext>(
    IServiceScopeFactory                           serviceScopeFactory,
    IHostApplicationLifetime                       lifetime,
    ILogger<MigrateDbContextAtStartup<TDbContext>> logger)
    : MigrateDbContextAtStartup<TDbContext>(serviceScopeFactory, logger)
    where TDbContext : DbContext
{
    protected override bool ShouldRun()
    {
        return true;
    }

    public override Task StartAsync(CancellationToken cancellationToken)
    {
        if (ShouldRun())
        {
            lifetime.ApplicationStarted.Register(PerformMigration);
        }

        return Task.CompletedTask;
    }
}