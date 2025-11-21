using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Painel.Investimento.Aplication.UseCasesProdutos;
using Painel.Investimento.Domain.Dtos;
using Painel.Investimento.Domain.Models;

namespace Painel.Investimento.API.Controllers
{
    [Route("api/produtoinvestimento")]
    [ApiController]
    public class ProdutoInvestimentoController : ControllerBase
    {
        private readonly ProdutoInvestimentoUseCase _useCase;
        private readonly RecomendarProdutosUseCase _recomenda;
        private readonly IMapper _mapper;

        public ProdutoInvestimentoController(ProdutoInvestimentoUseCase useCase, RecomendarProdutosUseCase recomenda, IMapper mapper)
        {
            _useCase = useCase;
            _recomenda = recomenda;
            _mapper = mapper;
        }

        /// <summary>
        /// Recomenda produtos de investimento para o cliente
        /// </summary>
        [HttpGet("produtos-recomendados/{clienteId}")]
        public async Task<ActionResult<IEnumerable<ProdutoRecomendadoDto>>> GetProdutosRecomendados(int clienteId)
        {
            try
            {
                var result = await _recomenda.ExecuteAsync(clienteId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao recomendar produtos: {ex.Message}");
            }
        }

        // ✅ POST: api/produtoinvestimento
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ProdutoInvestimentoRequestDto dto)
        {
            try
            {
                var produto = _mapper.Map<ProdutoInvestimento>(dto);

                var result = await _useCase.ExecuteAsync(
                    produto.Nome,
                    produto.Tipo,
                    produto.RentabilidadeAnual,
                    produto.Risco,
                    produto.Liquidez,
                    produto.Tributacao,
                    produto.Garantia,
                    produto.Descricao
                );

                var response = _mapper.Map<ProdutoInvestimentoResponseDto>(result);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao cadastrar produto de investimento: {ex.Message}");
            }
        }

        // ✅ GET: api/produtoinvestimento/{id}
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var produto = await _useCase.ObterPorIdAsync(id);
                if (produto == null) return NotFound("Produto não encontrado.");

                var response = _mapper.Map<ProdutoInvestimentoResponseDto>(produto);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao buscar produto de investimento: {ex.Message}");
            }
        }

        // ✅ GET: api/produtoinvestimento
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var produtos = await _useCase.ListarTodosAsync();
                var response = _mapper.Map<IEnumerable<ProdutoInvestimentoResponseDto>>(produtos);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao listar produtos de investimento: {ex.Message}");
            }
        }

        // ✅ PUT: api/produtoinvestimento/{id}
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Put(int id, [FromBody] ProdutoInvestimentoRequestDto dto)
        {
            try
            {
                var produtoAtualizado = await _useCase.AtualizarAsync(
                     id,
                     dto.Nome!,
                     dto.Tipo!,
                     dto.RentabilidadeAnual,
                     dto.Risco,
                     dto.Liquidez!,
                     dto.Tributacao!,
                     dto.Garantia!,
                     dto.Descricao!
                 );

                if (produtoAtualizado == null) return NotFound("Produto não encontrado para atualização.");

                var response = _mapper.Map<ProdutoInvestimentoResponseDto>(produtoAtualizado);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao atualizar produto de investimento: {ex.Message}");
            }
        }

        // ✅ DELETE: api/produtoinvestimento/{id}
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var removido = await _useCase.RemoverAsync(id);
                if (!removido) return NotFound("Produto não encontrado para remoção.");

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao remover produto de investimento: {ex.Message}");
            }
        }
    }
}
