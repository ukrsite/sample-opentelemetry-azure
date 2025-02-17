using Azure.Monitor.OpenTelemetry.AspNetCore;
using OpenTelemetry.Exporter;
using OpenTelemetry.Metrics;

namespace Sample.Metrics;

public class Program
{
    public static async Task<int> Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        ConfigureServices(builder.Services, builder.Configuration);
        var app = builder.Build();
        await app.RunAsync();

        return 0;
    }

    private static void ConfigureServices(IServiceCollection services, ConfigurationManager configuration)
    {
        services.AddSingleton<Operations.Metric>();
        services.AddHostedService<Orders.ItemsOrdered>();

        // TODO Move this to configuration to a secret
        var applicationInsightsConnectionString = "InstrumentationKey=468c58e9-327b-45c1-a995-96bbfe91e203;IngestionEndpoint=https://canadacentral-1.in.applicationinsights.azure.com/;LiveEndpoint=https://canadacentral.livediagnostics.monitor.azure.com/;ApplicationId=e25816df-2187-4605-883d-bd6f004e1aa8";

        services.AddOpenTelemetry().UseAzureMonitor(b =>
            {
                b.ConnectionString = applicationInsightsConnectionString;
            })
            .WithMetrics(metricBuilder => metricBuilder
                .AddMeter(Operations.Metric.ApplicationName)
                .AddConsoleExporter(builder => builder.Targets = ConsoleExporterOutputTargets.Console)
            );
    }
}
