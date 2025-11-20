using System.Net;
using System.Text.Json;

namespace Painel.Investimento.API.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (ArgumentException ex) // ✅ Erros de validação de negócio
            {
                _logger.LogWarning(ex, "Erro de validação capturado.");

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

                var response = new
                {
                    statusCode = context.Response.StatusCode,
                    message = "Erro de validação.",
                    detail = ex.Message
                };

                await context.Response.WriteAsync(JsonSerializer.Serialize(response));
            }
            catch (Exception ex) // ✅ Erros inesperados
            {
                _logger.LogError(ex, "Erro inesperado capturado pelo middleware.");

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                var response = new
                {
                    statusCode = context.Response.StatusCode,
                    message = "Ocorreu um erro inesperado. Tente novamente mais tarde.",
                    detail = ex.Message // ⚠️ em produção, evite expor detalhes sensíveis
                };

                await context.Response.WriteAsync(JsonSerializer.Serialize(response));
            }
        }
    }
}
