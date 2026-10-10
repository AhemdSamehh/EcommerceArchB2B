using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Polly;

namespace Ordering.API.Extensions
{
    public static class DbExtension
    {
        public static IHost MigrateDatabase<TContext>(this IHost host, Action<TContext, IServiceProvider> seeder)
            where TContext : DbContext
        {
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var logger = services.GetRequiredService<ILogger<TContext>>();
                var context = services.GetService<TContext>();

                try
                {
                    logger.LogInformation($"Started Db Migration: {typeof(TContext).Name}");
                    // retry strategy: catch any exception (network/socket/sql) and retry
                    var retry = Policy
                        .Handle<Exception>()
                        .WaitAndRetry(
                            retryCount: 8,
                            sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), // exponential backoff
                            onRetry: (exception, span, count) =>
                            {
                                logger.LogWarning(exception, "Retry {RetryCount} for {DbContext} after {Delay}s because of: {Message}",
                                    count, typeof(TContext).Name, span.TotalSeconds, exception.Message);
                            });

                    retry.Execute(() => CallSeeder(seeder, context, services, logger));
                    logger.LogInformation($"Migration Completed: {typeof(TContext).Name}");
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, $"An error occurred while migrating db: {typeof(TContext).Name}");
                }
            }
            return host;
        }


        private static void CallSeeder<TContext>(
    Action<TContext, IServiceProvider> seeder,
    TContext? context,
    IServiceProvider services,
    ILogger<TContext> logger)
    where TContext : DbContext
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            context.Database.Migrate();

            seeder(context, services);
        }

        private static void EnsureDatabaseExists<TContext>(TContext context, ILogger<TContext> logger) where TContext : DbContext
        {
            var connection = context.Database.GetDbConnection();
            var connectionString = connection.ConnectionString;

            var builder = new SqlConnectionStringBuilder(connectionString);
            var dbName = builder.InitialCatalog;
            if (string.IsNullOrWhiteSpace(dbName))
            {
                dbName = connection.Database;
            }

            if (string.IsNullOrWhiteSpace(dbName))
            {
                logger.LogWarning("Database name could not be determined from the connection; skipping EnsureDatabaseExists.");
                return;
            }

            // connect to master to create DB if it doesn't exist
            var masterBuilder = new SqlConnectionStringBuilder(connectionString)
            {
                InitialCatalog = "master"
            };

            var sanitized = dbName.Replace("'", "''");
            var createSql = $"IF DB_ID(N'{sanitized}') IS NULL CREATE DATABASE [{dbName}];";

            // targeted retry when SQL Server is not yet ready (e.g. container starting)
            var connectionRetry = Policy
                .Handle<SqlException>()
                .Or<InvalidOperationException>()
                .WaitAndRetry(
                    retryCount: 15,
                    sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(2),
                    onRetry: (ex, ts, count, ctx) =>
                    {
                        logger.LogWarning(ex, "Waiting for SQL Server to be ready (attempt {Attempt}).", count);
                    });

            // If retries fail, allow exception to bubble so outer retry can also attempt again.
            connectionRetry.Execute(() =>
            {
                using var conn = new SqlConnection(masterBuilder.ConnectionString);
                conn.Open();
                using var cmd = new SqlCommand(createSql, conn);
                cmd.CommandTimeout = 60;
                cmd.ExecuteNonQuery();
            });

            logger.LogInformation("Ensured database '{Database}' exists.", dbName);
        }
    }
}