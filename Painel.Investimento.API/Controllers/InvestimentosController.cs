using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Painel.Investimento.Aplication.UseCaseInvestimentos;
using Painel.Investimento.Aplication.UseCasesProdutos;
using Painel.Investimento.Domain.Dtos;
using Painel.Investimento.Domain.Models;

namespace Painel.Investimento.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InvestimentosController : ControllerBase
    {
        private readonly InvestimentosUseCase _useCase;
        private readonly ProdutoInvestimentoUseCase _produtoUseCase;
        private readonly IMapper _mapper;

        public InvestimentosController(InvestimentosUseCase useCase, ProdutoInvestimentoUseCase produtoUseCase,
            IMapper mapper)
        {
            _useCase = useCase;
            _produtoUseCase = produtoUseCase;
            _mapper = mapper;
        }




        // ✅ Registrar novo investimento
        [HttpPost]
        public async Task<ActionResult<InvestimentoDto>> Post([FromBody] CreateInvestimentoDto dto)
        {
            var investimento = await _useCase.RegistrarAsync(
                dto.ClienteId,
                dto.ProdutoInvestimentoId,
                dto.ValorInvestido,
                dto.DataInvestimento,
                dto.PrazoMeses
            );

            if (investimento == null)
                return BadRequest("Não foi possível registrar o investimento.");

            var investimentoDto = _mapper.Map<InvestimentoDto>(investimento);
            return CreatedAtAction(nameof(GetByIdInvestimmento), new { id = investimentoDto.Id }, investimentoDto);
        }

        // ✅ Registrar retirada
        [HttpPost("retirar-investimento")]
        public async Task<ActionResult<InvestimentoDto>> RetiraInvestimento([FromBody] RetiradaInvestimentoDto dto)
        {
            var investimento = await _useCase.RetirarInvestimentoAsync(
                dto.ClienteId,
                dto.ProdutoInvestimentoId,
                dto.ValorInvestido,
                 dto.DataInvestimento,
                 dto.PrazoMeses,
                dto.ValorRetirado ?? 0,
                dto.Crise
            );

            if (investimento == null)
                return BadRequest("Não há investimentos para retirada.");

            var investimentoDto = _mapper.Map<RetiradaInvestimentoDto>(investimento);
            return CreatedAtAction(nameof(GetByIdInvestimmento), new { id = investimentoDto.ClienteId }, investimentoDto);
        }

        // ✅ Obter investimento por Id
        [HttpGet("por-investimentoId/{id}")]
        public async Task<ActionResult<InvestimentoDto>> GetByIdInvestimmento(int id)
        {
            var investimento = await _useCase.ObterPorIdAsync(id);
            if (investimento == null)
                return NotFound();

            return Ok(_mapper.Map<InvestimentoDto>(investimento));
        }

        // ✅ Listar investimentos de um cliente
        [HttpGet("{clienteId}")]
        public async Task<ActionResult<IEnumerable<InvestimentoResumoDto>>> GetByCliente(int clienteId)
        {
            var investimentos = await _useCase.ListarPorClienteAsync(clienteId);

            Investimentos invest = investimentos.FirstOrDefault(p => p.ClienteId == clienteId);

            ProdutoInvestimento produtoinvestido = await _produtoUseCase.ObterPorIdAsync(invest.ProdutoInvestimentoId);

            var response = investimentos.Select(inv => new InvestimentoResumoDto
            {
                Id = inv.Id,
                Tipo = produtoinvestido.Nome ?? string.Empty,
                Valor = inv.ValorInvestido ?? 0,
                Rentabilidade = produtoinvestido.RentabilidadeAnual,
                Data = inv.DataInvestimento ?? DateTime.MinValue
            });

            return Ok(response);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<InvestimentoDto>> Put(int id, [FromBody] InvestimentoDto dto)
        {
            var investimento = await _useCase.AtualizarAsync(id, dto.ValorInvestido, dto.PrazoMeses);
            if (investimento == null)
                return NotFound();

            return Ok(_mapper.Map<InvestimentoDto>(investimento));
        }

        // ✅ Remover investimento
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var removido = await _useCase.RemoverAsync(id);
            if (!removido)
                return NotFound();

            return NoContent();
        }
    }
}
