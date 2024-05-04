using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Logging;

// ReSharper disable CheckNamespace
namespace Microsoft.EntityFrameworkCore;

internal static class DbContextMigrationHelpers
{
    /// <summary>
    ///     Processes the schema migration for the given <see cref="DbContext"/>
    /// </summary>
    /// <param name="dbMigrationContext">
    ///     The <see cref="DbContext"/> to detect and migrate the schema
    /// </param>
    /// <param name="logger">The <see cref="ILogger"/> instance for writing logs</param>
    public static void AutoMigrateDbSchema(this DbContext dbMigrationContext, ILogger logger = null)
    {
        var dbContextName = dbMigrationContext.GetType().Name;

        logger?.LogInformation("Getting pending migrations for {dbContextName}",
                               dbContextName);

        var pendingMigrations = dbMigrationContext.Database
                                                  .GetPendingMigrations()
                                                  .ToArray();

        logger?.LogInformation("{dbContextName} has {numberOfMigrations} pending migrations",
                               dbContextName,
                               pendingMigrations.Length);

        if (pendingMigrations.Any())
        {
            logger?.LogInformation("Migrating database for {dbContextName}",
                                   dbContextName);

            try
            {
                var migrator = dbMigrationContext.Database.GetService<IMigrator>();

                foreach (var pendingMigration in pendingMigrations)
                {
                    logger?.LogInformation("Executing migration {pendingMigration}...", pendingMigration);

                    migrator.Migrate(pendingMigration);

                    logger?.LogInformation("Migration {pendingMigration} executed.", pendingMigration);
                }
            }
            catch (Exception exception)
            {
                logger?.LogError(exception,
                                 "Error while executing migration for context {dbContextName}",
                                 dbContextName);

                throw;
            }
        }
    }
}