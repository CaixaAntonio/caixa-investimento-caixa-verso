using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Painel.Investimento.Aplication.UseCasesProdutos;
using Painel.Investimento.Domain.Dtos;
using Painel.Investimento.Domain.Models;

namespace Painel.Investimento.API.Controllers
{
    [Route("api/perfilproduto")]
    [ApiController]
    public class PerfilProdutoController : ControllerBase
    {
        private readonly PerfilProdutoUseCase _useCase;
        private readonly IMapper _mapper;

        public PerfilProdutoController(PerfilProdutoUseCase useCase, IMapper mapper)
        {
            _useCase = useCase;
            _mapper = mapper;
        }

        // ✅ POST: api/perfilproduto
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PerfilProdutoRequestDto dto)
        {
            var perfilProduto = await _useCase.VincularAsync(dto.PerfilDeRiscoId, dto.ProdutoInvestimentoId);

            var response = _mapper.Map<PerfilProdutoResponseDto>(perfilProduto);
            return Ok(response);
        }

        // ✅ GET: api/perfilproduto
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var perfilProdutos = await _useCase.ListarTodosAsync();
            var response = _mapper.Map<IEnumerable<PerfilProdutoResponseDto>>(perfilProdutos);
            return Ok(response);
        }

        // ✅ GET: api/perfilproduto/{perfilDeRiscoId}/{produtoInvestimentoId}
        [HttpGet("{perfilDeRiscoId:int}/{produtoInvestimentoId:int}")]
        public async Task<IActionResult> GetByIds(int perfilDeRiscoId, int produtoInvestimentoId)
        {
            var perfilProduto = await _useCase.ObterPorIdsAsync(perfilDeRiscoId, produtoInvestimentoId);
            if (perfilProduto == null) return NotFound();

            var response = _mapper.Map<PerfilProdutoResponseDto>(perfilProduto);
            return Ok(response);
        }

        // ✅ DELETE: api/perfilproduto/{perfilDeRiscoId}/{produtoInvestimentoId}
        [HttpDelete("{perfilDeRiscoId:int}/{produtoInvestimentoId:int}")]
        public async Task<IActionResult> Delete(int perfilDeRiscoId, int produtoInvestimentoId)
        {
            var removido = await _useCase.RemoverAsync(perfilDeRiscoId, produtoInvestimentoId);
            if (!removido) return NotFound();

            return NoContent();
        }
    }
}
