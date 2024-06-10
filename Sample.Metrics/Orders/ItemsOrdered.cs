using Sample.Metrics.Operations;
using System.Security.Cryptography;

namespace Sample.Metrics.Orders;

// Background service that publish random metrics in an interval
public class ItemsOrdered(ILogger<ItemsOrdered> logger, Metric metric) : BackgroundService
{
    private ILogger<ItemsOrdered> Logger { get; } = logger;
    private Metric Metric { get; } = metric;
    private TimeSpan interval = TimeSpan.FromMinutes(15);

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var itemsOrdered = RandomNumberGenerator.GetInt32(1, 9);
            Metric.ItemsOrdered(itemsOrdered);
            Logger.LogInformation("items ordered {ItemsOrdered}", itemsOrdered);
            await Task.Delay(interval, stoppingToken);
        }
    }
}