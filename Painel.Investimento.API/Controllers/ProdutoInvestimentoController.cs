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
            var result = await _recomenda.ExecuteAsync(clienteId);
            return Ok(result);
        }

        // ✅ POST: api/produtoinvestimento
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ProdutoInvestimentoRequestDto dto)
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

        // ✅ GET: api/produtoinvestimento/{id}
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var produto = await _useCase.ObterPorIdAsync(id);
            if (produto == null) return NotFound();

            var response = _mapper.Map<ProdutoInvestimentoResponseDto>(produto);
            return Ok(response);
        }

        // ✅ GET: api/produtoinvestimento
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var produtos = await _useCase.ListarTodosAsync();
            var response = _mapper.Map<IEnumerable<ProdutoInvestimentoResponseDto>>(produtos);
            return Ok(response);
        }

        // ✅ PUT: api/produtoinvestimento/{id}
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Put(int id, [FromBody] ProdutoInvestimentoRequestDto dto)
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

            if (produtoAtualizado == null) return NotFound();

            var response = _mapper.Map<ProdutoInvestimentoResponseDto>(produtoAtualizado);
            return Ok(response);
        }

        // ✅ DELETE: api/produtoinvestimento/{id}
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var removido = await _useCase.RemoverAsync(id);
            if (!removido) return NotFound();

            return NoContent();
        }
    }
}
