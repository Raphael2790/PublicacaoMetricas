using PublicacaoMetricas.Abstractions.Application;
using PublicacaoMetricas.Common;

namespace PublicacaoMetricas.Application;

public class ChecklistUseCase : IUseCase<CriarChecklist, ServiceResult>
{
    public async Task<ServiceResult> ExecuteAsync(CriarChecklist request)
    {
        await Task.Delay(1000);

        return ServiceResult.CreateSuccess();
    }
}