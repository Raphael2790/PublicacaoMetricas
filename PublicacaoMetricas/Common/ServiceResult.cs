namespace PublicacaoMetricas.Common;

public class ServiceResult
{
    public bool Success { get; set; }
    public string ErrorMessage { get; set; }
    
    public static ServiceResult CreateSuccess()
    {
        return new ServiceResult { Success = true };
    }
    
    public static ServiceResult CreateError(string errorMessage)
    {
        return new ServiceResult { Success = false, ErrorMessage = errorMessage };
    }
}