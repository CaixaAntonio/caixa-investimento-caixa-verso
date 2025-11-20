using Microsoft.AspNetCore.Mvc;
using Painel.Investimento.Aplication.UseCasesLogin;

namespace Painel.Investimento.API.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly LoginUseCase _loginUseCase;

        public AuthController(LoginUseCase loginUseCase)
        {
            _loginUseCase = loginUseCase;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest dto)
        {
            try
            {
                var token = await _loginUseCase.ExecuteAsync(dto.Username, dto.Password);
                return Ok(new
                {
                    access_token = token,
                    token_type = "Bearer",
                    expires_in = 7200
                });
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized(new { message = "Credenciais inválidas" });
            }
        }
    }

    public record LoginRequest(string Username, string Password);
}
