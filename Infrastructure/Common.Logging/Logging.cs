using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Serilog.Exceptions;
using Serilog.Sinks.Elasticsearch;


namespace Common.Logging
{
    public static class Logging
    {
        /// <summary>
        /// Action used to configure Serilog from a <see cref="HostBuilderContext"/>.
        /// Intended to be used like: Host.CreateDefaultBuilder(args).UseSerilog(Logging.ConfigureLogger);
        /// </summary>
        public static Action<HostBuilderContext, LoggerConfiguration> ConfigureLogger =>
            (context, loggerConfiguration) =>
            {
                var env = context?.HostingEnvironment ?? throw new ArgumentNullException(nameof(context));

                // Enable Serilog internal self-log to console for troubleshooting sink/format issues
                Serilog.Debugging.SelfLog.Enable(Console.Error);

                loggerConfiguration
                    .MinimumLevel.Information()
                    .Enrich.FromLogContext()
                    .Enrich.WithMachineName()
                    .Enrich.WithProperty("ApplicationName", env.ApplicationName)
                    .Enrich.WithProperty("Environment", env.EnvironmentName)
                    .Enrich.WithExceptionDetails()
                    .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
                    .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Warning)
                    .WriteTo.Console();

                if (env.IsDevelopment())
                {
                    // Lower noise for infrastructure while enabling debug for service namespaces
                    loggerConfiguration.MinimumLevel.Override("Catalog", LogEventLevel.Debug);
                    loggerConfiguration.MinimumLevel.Override("Basket", LogEventLevel.Debug);
                    loggerConfiguration.MinimumLevel.Override("Discount", LogEventLevel.Debug);
                    loggerConfiguration.MinimumLevel.Override("Ordering", LogEventLevel.Debug);
                    loggerConfiguration.MinimumLevel.Override("Payment", LogEventLevel.Debug);
                    // loggerConfiguration.MinimumLevel.Override("Identity", LogEventLevel.Debug);
                }

                var elasticUrl = context.Configuration["ElasticConfiguration:Uri"];
                var elasticUser = context.Configuration["ElasticConfiguration:Username"];
                var elasticPassword = context.Configuration["ElasticConfiguration:Password"];

                if (!string.IsNullOrWhiteSpace(elasticUrl))
                {
                    loggerConfiguration.WriteTo.Elasticsearch(
                        new ElasticsearchSinkOptions(new Uri(elasticUrl))
                        {
                            AutoRegisterTemplate = true,

                            // Important for Elasticsearch 8
                            TypeName = null,

                            // Keep index names lowercase and time-partitioned by month
                            IndexFormat = $"ecommerceb2bindexed-logs-{env.EnvironmentName?.ToLowerInvariant()}-{DateTime.UtcNow:yyyy-MM}",

                            MinimumLogEventLevel = LogEventLevel.Debug,

                            ModifyConnectionSettings = cs => cs
                                .BasicAuthentication(elasticUser, elasticPassword)
                                .ServerCertificateValidationCallback((o, cert, chain, errors) => true),

                            EmitEventFailure = EmitEventFailureHandling.WriteToSelfLog
                                             | EmitEventFailureHandling.RaiseCallback,
                        });
                }
            };
    }
}
