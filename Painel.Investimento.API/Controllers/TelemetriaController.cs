using Microsoft.AspNetCore.Mvc;
using Painel.Investimento.Application.Services;
using Painel.Investimento.Domain.Dtos.TelemetriaDto;

namespace Painel.Investimento.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TelemetriaController : ControllerBase
    {
        private readonly ITelemetriaService _telemetriaService;

        public TelemetriaController(ITelemetriaService telemetriaService)
        {
            _telemetriaService = telemetriaService;
        }

        [HttpGet]
        public ActionResult<TelemetriaResponse> GetTelemetria([FromQuery] DateTime inicio, [FromQuery] DateTime fim)
        {
            try
            {
                var relatorio = _telemetriaService.ObterRelatorio(inicio, fim);

                if (relatorio == null)
                    return NotFound("Nenhum dado de telemetria encontrado para o período informado.");

                return Ok(relatorio);
            }
            catch (ArgumentException ex)
            {
                // Exemplo: se parâmetros de data forem inválidos
                return BadRequest($"Parâmetros inválidos: {ex.Message}");
            }
            catch (Exception ex)
            {
                // Captura qualquer erro inesperado
                return StatusCode(500, $"Erro ao obter telemetria: {ex.Message}");
            }
        }
    }
}
