using Microsoft.EntityFrameworkCore;

namespace Microsoft.Extensions.DependencyInjection;

internal static class ServiceCollectionForDbMigrationsExtensions
{
    public static IServiceCollection AddAutoMigrationForDbContextAtStartup<TDbContext>(
        this IServiceCollection serviceCollection)
        where TDbContext : DbContext
    {
        Guards.GuardEnsureDbContextRegistered<TDbContext>(serviceCollection);

        serviceCollection.AddHostedService<AutoMigrateDbContextAtStartup<TDbContext>>();

        return serviceCollection;
    }

    public static IServiceCollection AddAutoMigrationForDbContextAfterAppStarted<TDbContext>(
        this IServiceCollection serviceCollection)
        where TDbContext : DbContext
    {
        Guards.GuardEnsureDbContextRegistered<TDbContext>(serviceCollection);

        serviceCollection.AddHostedService<AutoMigrateDbContextAfterAppStarted<TDbContext>>();

        return serviceCollection;
    }

    internal static class Guards
    {
        internal static void GuardEnsureDbContextRegistered<TDbContext>(IServiceCollection serviceCollection)
            where TDbContext : DbContext
        {
            if (serviceCollection.FirstOrDefault(_ => _.ServiceType == typeof(TDbContext)) is null)
            {
                throw new
                    InvalidOperationException($"The DbContext {typeof(TDbContext).Name} must be registered to the IServiceCollection before auto-migration can be registered");
            }
        }
    }
}