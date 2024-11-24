using PublicacaoMetricas.Common;

namespace PublicacaoMetricas.Abstractions.Infrastructure;

public interface IMessageService
{
    Task<ServiceResult> SendMessageAsync(string message);
}