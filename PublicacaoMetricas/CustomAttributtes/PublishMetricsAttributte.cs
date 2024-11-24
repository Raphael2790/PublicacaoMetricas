namespace PublicacaoMetricas.CustomAttributtes;

[AttributeUsage(AttributeTargets.Method)]
public class PublishMetricsAttributte : Attribute
{
    public Type MessageServiceType { get; set; }
    public string MetricName { get; set; }
    
    public PublishMetricsAttributte(Type messageServiceType, string metricName)
    {
        MessageServiceType = messageServiceType;
        MetricName = metricName;
    }
}