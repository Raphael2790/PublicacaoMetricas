using PublicacaoMetricas.Abstractions.Infrastructure;
using PublicacaoMetricas.CustomAttributtes;

namespace PublicacaoMetricas.Abstractions.Application;

public interface IUseCase<TRequest, TResponse>
{
    [PublishMetricsAttributte(typeof(IMessageService), "MetricName")]
    Task<TResponse> ExecuteAsync(TRequest request);
}