using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

// ReSharper disable CheckNamespace
namespace Microsoft.EntityFrameworkCore;

internal abstract class DbContextSeedingStartupTask<TDbContext>
    : IHostedService
    where TDbContext : DbContext
{
    private readonly ILogger                  _logger;
    private readonly IServiceScopeFactory     _serviceScopeFactory;
    private readonly IHostApplicationLifetime _lifetime;

    protected DbContextSeedingStartupTask(IServiceScopeFactory     serviceScopeFactory,
                                          IHostApplicationLifetime lifetime,
                                          ILoggerFactory           loggerFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _lifetime            = lifetime;
        _logger              = loggerFactory.CreateLogger(GetType());
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _lifetime.ApplicationStarted.Register(PerformSeeding);

        return Task.CompletedTask;
    }

    protected abstract Task Seed(IServiceProvider serviceProvider);

    private void PerformSeeding()
    {
        using (var scope = _serviceScopeFactory.CreateScope())
        {
            Seed(scope.ServiceProvider).Wait();
        }
    }

    public virtual Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}