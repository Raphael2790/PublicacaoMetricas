using Amazon.SQS;
using Castle.DynamicProxy;
using Microsoft.Extensions.DependencyInjection;
using PublicacaoMetricas.Abstractions.Application;
using PublicacaoMetricas.Abstractions.Infrastructure;
using PublicacaoMetricas.Application;
using PublicacaoMetricas.Common;
using PublicacaoMetricas.Infrastructure;
using PublicacaoMetricas.Interceptors;

namespace PublicacaoMetricas.Configuration;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        var proxyGenerator = new ProxyGenerator();
        services.AddSingleton(proxyGenerator);
        
        services.AddSingleton<MetricInterceptor>();
        services.AddSingleton<MetricInterceptorByAttribute>();
        services.AddSingleton<IMessageService, MessageService>();

        services.AddSingleton<IUseCase<CriarChecklist, ServiceResult>>(provider =>
        {
            var interceptor = provider.GetRequiredService<MetricInterceptorByAttribute>();
            var useCase = new ChecklistUseCase();
            return proxyGenerator.CreateInterfaceProxyWithTarget<IUseCase<CriarChecklist, ServiceResult>>(useCase, interceptor);
        });
    }
}