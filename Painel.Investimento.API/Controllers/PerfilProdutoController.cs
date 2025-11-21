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
            try
            {
                var perfilProduto = await _useCase.VincularAsync(dto.PerfilDeRiscoId, dto.ProdutoInvestimentoId);
                if (perfilProduto == null)
                    return BadRequest("Não foi possível vincular o perfil ao produto.");

                var response = _mapper.Map<PerfilProdutoResponseDto>(perfilProduto);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao vincular perfil ao produto: {ex.Message}");
            }
        }

        // ✅ GET: api/perfilproduto
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var perfilProdutos = await _useCase.ListarTodosAsync();
                var response = _mapper.Map<IEnumerable<PerfilProdutoResponseDto>>(perfilProdutos);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao listar perfis de produto: {ex.Message}");
            }
        }

        // ✅ GET: api/perfilproduto/{perfilDeRiscoId}/{produtoInvestimentoId}
        [HttpGet("{perfilDeRiscoId:int}/{produtoInvestimentoId:int}")]
        public async Task<IActionResult> GetByIds(int perfilDeRiscoId, int produtoInvestimentoId)
        {
            try
            {
                var perfilProduto = await _useCase.ObterPorIdsAsync(perfilDeRiscoId, produtoInvestimentoId);
                if (perfilProduto == null) return NotFound("Perfil ou produto não encontrado.");

                var response = _mapper.Map<PerfilProdutoResponseDto>(perfilProduto);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao buscar perfil de produto: {ex.Message}");
            }
        }

        // ✅ DELETE: api/perfilproduto/{perfilDeRiscoId}/{produtoInvestimentoId}
        [HttpDelete("{perfilDeRiscoId:int}/{produtoInvestimentoId:int}")]
        public async Task<IActionResult> Delete(int perfilDeRiscoId, int produtoInvestimentoId)
        {
            try
            {
                var removido = await _useCase.RemoverAsync(perfilDeRiscoId, produtoInvestimentoId);
                if (!removido) return NotFound("Perfil ou produto não encontrado para remoção.");

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao remover perfil de produto: {ex.Message}");
            }
        }
    }
}
