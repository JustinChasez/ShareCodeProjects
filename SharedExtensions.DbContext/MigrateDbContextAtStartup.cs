using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Microsoft.EntityFrameworkCore;

/// <summary>
///     Represents the task to detect and migrate the given database context at startup
/// </summary>
/// <typeparam name="TDbContext">The type of the <see cref="DbContext"/> to migrate</typeparam>
internal abstract class MigrateDbContextAtStartup<TDbContext>
    : IHostedService
    where TDbContext : DbContext
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly ILogger              _logger;

    protected MigrateDbContextAtStartup(IServiceScopeFactory serviceScopeFactory,
                                        ILoggerFactory       loggerFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _logger              = loggerFactory.CreateLogger(GetType());
    }

    protected abstract bool                 ShouldRun();

    public virtual Task StartAsync(CancellationToken cancellationToken)
    {
        if (!ShouldRun())
            return Task.CompletedTask;

        PerformMigration();

        return Task.CompletedTask;
    }

    protected virtual void PerformMigration()
    {
        using (var scope = _serviceScopeFactory.CreateScope())
        {
            using (var dbContext = scope.ServiceProvider.GetRequiredService<TDbContext>())
            {
                dbContext.AutoMigrateDbSchema(_logger);
            }
        }
    }

    public virtual Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}