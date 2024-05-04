using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Microsoft.EntityFrameworkCore;

/// <summary>
///     Represents the task to automatically migrate the given database context at startup
/// </summary>
/// <typeparam name="TDbContext">The type of the <see cref="DbContext"/> to migrate</typeparam>
internal class AutoMigrateDbContextAtStartup<TDbContext>(
    IServiceScopeFactory                               serviceScopeFactory,
    ILogger<AutoMigrateDbContextAtStartup<TDbContext>> logger)
    : MigrateDbContextAtStartup<TDbContext>(serviceScopeFactory, logger)
    where TDbContext : DbContext
{
    protected override bool ShouldRun()
    {
        return true;
    }
}

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