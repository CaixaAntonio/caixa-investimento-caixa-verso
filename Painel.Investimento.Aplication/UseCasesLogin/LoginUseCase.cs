using Painel.Investimento.Domain.Services;

namespace Painel.Investimento.Aplication.UseCasesLogin
{
    public class LoginUseCase
    {
        private readonly IAuthService _authService;

        public LoginUseCase(IAuthService authService)
        {
            _authService = authService;
        }

        public async Task<string> ExecuteAsync(string username, string password)
        {
            var valid = await _authService.ValidateCredentialsAsync(username, password);
            if (!valid)
                throw new UnauthorizedAccessException("Credenciais inválidas.");

            return await _authService.GenerateTokenAsync(username, "admin");
        }
    }
}
