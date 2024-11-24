using System.Reflection;
using System.Text.Json;
using Castle.DynamicProxy;
using PublicacaoMetricas.Common;
using PublicacaoMetricas.CustomAttributtes;

namespace PublicacaoMetricas.Interceptors;

public class MetricInterceptorByAttribute : IInterceptor
{
    private readonly IServiceProvider _serviceProvider;

    public MetricInterceptorByAttribute(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    
    public void Intercept(IInvocation invocation)
    {
        // Verifica se o método possui o atributo PublishMetricsAttribute
        var method = invocation.Method;
        var attribute = method.GetCustomAttribute<PublishMetricsAttributte>();
        if (attribute != null)
        {
            Console.WriteLine($"[MetricsInterceptor] Capturando métrica: {attribute.MetricName}");

            // Executa o método original e captura o retorno
            invocation.ReturnValue = InterceptAsync<ServiceResult>(invocation, attribute);

            Console.WriteLine($"[MetricsInterceptor] Métrica publicada: {attribute.MetricName}");
        }
        else
        {
            // Sem atributo, apenas executa o método original
            invocation.Proceed();
        }
    }
    
    private async Task<T> InterceptAsync<T>(IInvocation invocation, PublishMetricsAttributte attribute)
    {
        try
        {
            // Chama o método original de forma assíncrona.
            invocation.Proceed();
            if (invocation.ReturnValue is Task<T> task)
            {
                T result = await task; // Aguarda e obtém o resultado da Task<T>.
                Console.WriteLine($"[AOP] Resultado do método assíncrono: {result}");
                
                var resultado = result as ServiceResult;
                
                Console.WriteLine($"[AOP] Método {invocation.Method.Name} executado com sucesso. Retorno: {resultado}");
            
                var metrica = new MetricMessage(attribute.MetricName, resultado.Success ? 1: 0, resultado.Success ? 0 : 1, 1);
            
                // Obtém o serviço de mensagens
                var messageService = _serviceProvider.GetService(attribute.MessageServiceType);
                if (messageService == null)
                {
                    throw new InvalidOperationException($"Serviço do tipo {attribute.MessageServiceType} não registrado.");
                }
                
                
                // Envia a métrica para o serviço
                var json = JsonSerializer.Serialize(metrica);
                var sendMetricsMethod = attribute.MessageServiceType.GetMethod("SendMessageAsync");
                sendMetricsMethod?.Invoke(messageService, [json]);
                
                return result;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[AOP] Erro no método assíncrono: {ex.Message}");
            throw;
        }

        return default!;
    }
}