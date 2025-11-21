using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Painel.Investimento.Application.Services;

public class TelemetriaMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<TelemetriaMiddleware> _logger;

    public TelemetriaMiddleware(RequestDelegate next, ILogger<TelemetriaMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, ITelemetriaService telemetriaService)
    {
        var stopwatch = Stopwatch.StartNew();

        try
        {
            // Executa o próximo middleware da pipeline
            await _next(context);
        }
        catch (Exception ex)
        {
            // Loga erro crítico durante execução da requisição
            _logger.LogError(ex, "Erro durante a execução da requisição em {Path}", context.Request.Path);

            // Repropaga a exceção para não engolir o erro da aplicação
            throw;
        }
        finally
        {
            stopwatch.Stop();

            try
            {
                // 🔹 Nome do serviço baseado no path
                var nomeServico = context.Request.Path.Value?.Trim('/').ToLower() ?? "desconhecido";

                telemetriaService.RegistrarChamada(nomeServico, stopwatch.ElapsedMilliseconds);

                // Loga informação de telemetria
                _logger.LogInformation("Chamada registrada para {Servico} em {Tempo}ms", nomeServico, stopwatch.ElapsedMilliseconds);
            }
            catch (Exception ex)
            {
                // Se der erro ao registrar telemetria, não deve quebrar a requisição
                _logger.LogWarning(ex, "Falha ao registrar telemetria para {Path}", context.Request.Path);
            }
        }
    }
}
