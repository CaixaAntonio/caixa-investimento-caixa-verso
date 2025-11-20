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
            var relatorio = _telemetriaService.ObterRelatorio(inicio, fim);
            return Ok(relatorio);
        }
    }
}
