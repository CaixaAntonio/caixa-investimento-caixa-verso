namespace Painel.Investimento.Domain.Services
{
    public interface IAuthService
    {
        Task<string> GenerateTokenAsync(string username, string role);
        Task<bool> ValidateCredentialsAsync(string username, string password);
    }
}
