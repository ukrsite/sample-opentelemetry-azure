using System.Diagnostics.Metrics;

namespace Sample.Metrics.Operations;

public class Metric
{
    public const string ApplicationName = "restaurant-order-svc";
    public const string MetricName = $"{ApplicationName}-";
    private static readonly Meter Meter = new(ApplicationName, "0.0.1");
    
    // a counter that goes only up
    private static readonly Counter<int> ItemsOrderedTotal = Meter.CreateCounter<int>($"{MetricName}items-ordered", "item", "Items ordered total");
    
    public void ItemsOrdered(int amount)
    {
        ItemsOrderedTotal.Add(amount);
    }
}
