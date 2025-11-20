using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Painel.Investimento.Domain.Dtos;
using Painel.Investimento.Aplication.useCaseSimulacoes;

namespace Painel.Investimento.API.Controllers
{
    [ApiController]
    [Route("api")]
    public class SimulacoesController : ControllerBase
    {
        private readonly ConsultarSimulacoesAgrupadasUseCase _useCase;
        private readonly SimularInvestimentoUseCase _simularInvestimento;
        private readonly ConsultarHistoricoSimulacoesUseCase _consultarHistorico;
        private readonly IMapper _mapper;

        public SimulacoesController(ConsultarSimulacoesAgrupadasUseCase useCase, SimularInvestimentoUseCase simularInvestimento,
            ConsultarHistoricoSimulacoesUseCase consultarHistorico, IMapper mapper)
        {
            _useCase = useCase;
            _simularInvestimento = simularInvestimento;
            _consultarHistorico = consultarHistorico;
            _mapper = mapper;
        }

        [HttpPost("simular-investimento")]
        public async Task<ActionResult<SimulacaoInvestimentoResponse>> Simular([FromBody] SimulacaoInvestimentoRequest request)
        {
            var useCaseRequest = _mapper.Map<SimulacaoInvestimentoRequest>(request);

            var useCaseResponse = await _simularInvestimento.ExecuteAsync(useCaseRequest);

            var response = _mapper.Map<SimulacaoInvestimentoResponse>(useCaseResponse);

            return Ok(response);
        }

        /// <summary>
        /// Retorna todas as simulações realizadas
        /// </summary>
        [HttpGet("simulacoes")]
        public async Task<ActionResult<IEnumerable<SimulacaoResumoDto>>> GetSimulacoes()
        {
            var result = await _simularInvestimento.ListarTodasAsync();
            return Ok(result);
        }


        [HttpGet("simulacoes/{clienteId}")]
        public async Task<ActionResult<SimulacaoHistoricoResponse>> GetHistorico(int clienteId)
        {
            var useCaseResponse = await _consultarHistorico.ExecuteAsync(clienteId);

            if (useCaseResponse.Simulacoes == null || !useCaseResponse.Simulacoes.Any())
                return NotFound("Nenhuma simulação encontrada para este cliente.");

            var response = _mapper.Map<SimulacaoHistoricoResponse>(useCaseResponse);

            return Ok(response);
        }

        /// <summary>
        /// Endpoint que calcula a rentabilidade percentual e verifica se é rentável
        /// </summary>
        /// <param name="id">Id da simulação</param>
        /// <param name="minimoPercentual">Percentual mínimo esperado</param>
        /// <returns></returns>
        [HttpGet("{id}/rentabilidade")]
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
        }

        /// <summary>
        /// Endpoint retorna Simulaçoes por dia e produto
        /// </summary>
        /// <returns></returns>

        [HttpGet("simulacoes/por-produto-dia")]
        public async Task<IActionResult> GetSimulacoesAgrupadas()
        {
            var result = await _useCase.ExecuteAsync();
            return Ok(result);
        }
    }
}
