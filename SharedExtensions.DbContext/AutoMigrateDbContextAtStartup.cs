using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Microsoft.EntityFrameworkCore;

/// <summary>
///     Represents the task to automatically migrate the given database context at startup
/// </summary>
/// <typeparam name="TDbContext">The type of the <see cref="DbContext"/> to migrate</typeparam>
internal class AutoMigrateDbContextAtStartup<TDbContext>(
    IServiceScopeFactory serviceScopeFactory,
    ILoggerFactory       loggerFactory)
    : MigrateDbContextAtStartup<TDbContext>(serviceScopeFactory, loggerFactory)
    where TDbContext : DbContext
{
    protected override bool ShouldRun()
    {
        return true;
    }
}