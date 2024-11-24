using System.Text.Json;
using Castle.DynamicProxy;
using PublicacaoMetricas.Abstractions.Infrastructure;
using PublicacaoMetricas.Common;

namespace PublicacaoMetricas.Interceptors;

public class MetricInterceptor(IMessageService messageService) : IInterceptor
{
    private readonly IMessageService _messageService = messageService;

    public void Intercept(IInvocation invocation)
    {
        try
        {
            if (!typeof(Task).IsAssignableFrom(invocation.Method.ReturnType)) return;
            
            // Verifica se o método é uma Task ou Task<T> e chama o interceptor assíncrono.
            invocation.ReturnValue = InterceptAsync<ServiceResult>(invocation);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[AOP] Erro no método {invocation.Method.Name}: {ex.Message}");
            throw;
        }
        finally
        {
            Console.WriteLine($"[AOP] Método {invocation.Method.Name} finalizado.");
        }
    }
    
    private async Task<T> InterceptAsync<T>(IInvocation invocation)
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
            
                var metrica = new MetricMessage("checklist", resultado.Success ? 1: 0, resultado.Success ? 0 : 1, 1);
                    
                var json = JsonSerializer.Serialize(metrica);
                
                await _messageService.SendMessageAsync(json);
                
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