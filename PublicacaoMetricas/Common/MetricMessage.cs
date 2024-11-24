namespace PublicacaoMetricas.Common;

public class MetricMessage : Message
{
    public string MetricName { get; set; }
    public int Success { get; set; }    
    public int Error { get; set; }
    public int Total { get; set; }
    
    public MetricMessage(string metricName, int success, int error, int total)
    {
        MetricName = metricName;
        Success = success;
        Error = error;
        Total = total;
    }
}