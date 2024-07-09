using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

// ReSharper disable CheckNamespace
namespace Microsoft.EntityFrameworkCore;

internal abstract class DataSeedingStartupTask : IHostedService
{
    private readonly IServiceScopeFactory     _serviceScopeFactory;
    private readonly IHostApplicationLifetime _lifetime;
    private readonly ILogger                  _logger;

    protected DataSeedingStartupTask(IServiceScopeFactory     serviceScopeFactory,
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
        using var scope = _serviceScopeFactory.CreateScope();

        Seed(scope.ServiceProvider).Wait();
    }

    public virtual Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}