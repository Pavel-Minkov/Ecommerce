using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Serilog.Exceptions;
using Serilog.Sinks.Elasticsearch;

namespace Common.Logging
{
    public static class Logging
    {
        public static Action<HostBuilderContext, LoggerConfiguration> ConfigureLogging = (context, config) =>
        { 
            var env = context.HostingEnvironment;
            var configuration = context.Configuration;

            config
                .MinimumLevel.Information()
                .Enrich.FromLogContext()
                .Enrich.WithProperty("ApplicationName", env.ApplicationName)
                .Enrich.WithProperty("EnvironmentName", env.EnvironmentName)
                .Enrich.WithExceptionDetails()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Information)
                .MinimumLevel.Override("System", LogEventLevel.Warning)
                .WriteTo.Console();

            if (env.IsDevelopment())
            {
                config.MinimumLevel.Debug();
            }
            var elasticUri = configuration.GetValue<string>("ElasticConfiguration:Uri");

            if (!string.IsNullOrWhiteSpace(elasticUri))
            {
                config.WriteTo.Elasticsearch(
                    new ElasticsearchSinkOptions(new Uri(elasticUri))
                    {
                        AutoRegisterTemplate = true,
                        AutoRegisterTemplateVersion = AutoRegisterTemplateVersion.ESv8,

                        // Key improvement: per-service + per-env index
                        IndexFormat =
                            $"ecommerce-{env.ApplicationName?.ToLower().Replace('.', '-')}-" +
                            $"{env.EnvironmentName?.ToLower()}-" +
                            $"{DateTime.UtcNow:yyyy.MM.dd}",

                        // Keep debug for dev; override via config for prod
                        MinimumLogEventLevel = env.IsDevelopment() ? LogEventLevel.Debug : LogEventLevel.Information
                    });
            }
        };
    }
}
