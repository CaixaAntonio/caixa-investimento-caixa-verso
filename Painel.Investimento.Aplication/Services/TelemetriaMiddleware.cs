using System.Diagnostics;
using System.Net.Http;
using Microsoft.AspNetCore.Http;
using Painel.Investimento.Application.Services;

public class TelemetriaMiddleware
{
    private readonly RequestDelegate _next;

    public TelemetriaMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, ITelemetriaService telemetriaService)
    {
        var stopwatch = Stopwatch.StartNew();

        await _next(context);

        stopwatch.Stop();

        // 🔹 Nome do serviço baseado no path
        var nomeServico = context.Request.Path.Value?.Trim('/').ToLower() ?? "desconhecido";

        telemetriaService.RegistrarChamada(nomeServico, stopwatch.ElapsedMilliseconds);
    }
}
