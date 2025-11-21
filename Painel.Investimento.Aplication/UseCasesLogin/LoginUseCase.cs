using Microsoft.Extensions.Logging;
using Painel.Investimento.Domain.Services;
using System;
using System.Threading.Tasks;

namespace Painel.Investimento.Aplication.UseCasesLogin
{
    public class LoginUseCase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<LoginUseCase> _logger;

        public LoginUseCase(IAuthService authService, ILogger<LoginUseCase> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        public async Task<string> ExecuteAsync(string username, string password)
        {
            try
            {
                _logger.LogInformation("Tentativa de login para usuário {Username}", username);

                var valid = await _authService.ValidateCredentialsAsync(username, password);
                if (!valid)
                {
                    _logger.LogWarning("Credenciais inválidas para usuário {Username}", username);
                    throw new UnauthorizedAccessException("Credenciais inválidas.");
                }

                var token = await _authService.GenerateTokenAsync(username, "admin");

                _logger.LogInformation("Login bem-sucedido para usuário {Username}", username);

                return token;
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning(ex, "Falha de autenticação para usuário {Username}", username);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado durante login para usuário {Username}", username);
                throw;
            }
        }
    }
}
