using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Painel.Investimento.Domain.Dtos;
using Painel.Investimento.Aplication.useCaseSimulacoes;
using Microsoft.AspNetCore.Authorization;

namespace Painel.Investimento.API.Controllers
{
    [ApiController]
    [Route("/[controller]")]

    public class SimulacoesController : ControllerBase
    {
        private readonly ConsultarSimulacoesAgrupadasUseCase _useCase;
        private readonly SimularInvestimentoUseCase _simularInvestimento;
        private readonly ConsultarHistoricoSimulacoesUseCase _consultarHistorico;
        private readonly IMapper _mapper;

        public SimulacoesController(
            ConsultarSimulacoesAgrupadasUseCase useCase,
            SimularInvestimentoUseCase simularInvestimento,
            ConsultarHistoricoSimulacoesUseCase consultarHistorico,
            IMapper mapper)
        {
            _useCase = useCase;
            _simularInvestimento = simularInvestimento;
            _consultarHistorico = consultarHistorico;
            _mapper = mapper;
        }

        [HttpPost("/simular-investimento")]
        [Authorize]
        public async Task<ActionResult<SimulacaoInvestimentoResponse>> Simular([FromBody] SimulacaoInvestimentoRequest request)
        {
            try
            {
                var useCaseRequest = _mapper.Map<SimulacaoInvestimentoRequest>(request);
                var useCaseResponse = await _simularInvestimento.ExecuteAsync(useCaseRequest);
                var response = _mapper.Map<SimulacaoInvestimentoResponse>(useCaseResponse);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao simular investimento: {ex.Message}");
            }
        }

        /// <summary>
        /// Retorna todas as simulações realizadas
        /// </summary>
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<SimulacaoResumoDto>>> GetSimulacoes()
        {
            try
            {
                var result = await _simularInvestimento.ListarTodasAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao listar simulações: {ex.Message}");
            }
        }

        [HttpGet("/{clienteId}")]
        [Authorize]
        public async Task<ActionResult<SimulacaoHistoricoResponse>> GetHistorico(int clienteId)
        {
            try
            {
                var useCaseResponse = await _consultarHistorico.ExecuteAsync(clienteId);

                if (useCaseResponse.Simulacoes == null || !useCaseResponse.Simulacoes.Any())
                    return NotFound("Nenhuma simulação encontrada para este cliente.");

                var response = _mapper.Map<SimulacaoHistoricoResponse>(useCaseResponse);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao consultar histórico de simulações: {ex.Message}");
            }
        }

        /// <summary>
        /// Endpoint que calcula a rentabilidade percentual e verifica se é rentável
        /// </summary>
        [HttpGet("{id}/rentabilidade")]
        [Authorize]
        public async Task<IActionResult> GetRentabilidade(int id, [FromQuery] decimal minimoPercentual)
        {
            try
            {
                var result = await _simularInvestimento.CalcularRentabilidadeAsync(id, minimoPercentual);
                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao calcular rentabilidade: {ex.Message}");
            }
        }

        /// <summary>
        /// Endpoint retorna Simulações por dia e produto
        /// </summary>
        [HttpGet("/simulacoes/por-produto-dia")]
        [Authorize]
        public async Task<IActionResult> GetSimulacoesAgrupadas()
        {
            try
            {
                var result = await _useCase.ExecuteAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao consultar simulações agrupadas: {ex.Message}");
            }
        }
    }
}
