using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Painel.Investimento.Aplication.UseCaseInvestimentos;
using Painel.Investimento.Aplication.UseCasesCadastros;
using Painel.Investimento.Domain.Dtos;
using Painel.Investimento.Domain.Models;
using Painel.Investimento.Domain.Repository.Abstract;
using Painel.Investimento.Domain.Valueobjects;

namespace Painel.Investimento.API.Controllers
{

    [ApiController]
    [Route("api")]
    public class ClienteController : ControllerBase
    {
        private readonly ClienteUseCase _useCase;
        private readonly IInvestimentosRepository _investimentoRepo;
        private readonly CalcularPerfilDeRiscoUseCase _calcularPerfilDeRisco;
        private readonly IMapper _mapper;

        public ClienteController(ClienteUseCase useCase, IInvestimentosRepository investimentoRepo,
            CalcularPerfilDeRiscoUseCase calcularPerfilDeRisco, IMapper mapper)
        {
            _useCase = useCase;
            _investimentoRepo = investimentoRepo;
            _calcularPerfilDeRisco = calcularPerfilDeRisco;
            _mapper = mapper;
        }



        /// <summary>
        /// Calcula e retorna o perfil de risco do cliente com base nos seus investimentos.
        /// </summary>
        [HttpGet("perfil-risco-dinamico/{clienteId:int}")]
        public async Task<ActionResult<object>> GetPerfilRisco(int clienteId)
        {
            var investimentos = await _investimentoRepo.ObterPorClienteAsync(clienteId);
            if (investimentos == null || !investimentos.Any())
                return NotFound("Nenhum investimento encontrado para este cliente.");

            var score = _calcularPerfilDeRisco.CalcularPontuacao(investimentos);
            var perfil = _calcularPerfilDeRisco.ClassificarPerfil(score);

            return Ok(new
            {
                ClienteId = clienteId,
                Perfil = perfil.Split(',')[0].Trim(),
                Pontuacao = score,
                Descrição = perfil.Split(',')[1].Trim()


            });
        }

        // POST: api/cliente
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ClienteRequestDto dto)
        {
            var email = new Email(dto.Email);
            var celular = new Celular(dto.Celular);
            var cpf = new Cpf(dto.Cpf);
            var dataNascimento = new DataDeNascimento(dto.DataDeNascimento);

            var cliente = await _useCase.ExecuteAsync(
                dto.Id,
                dto.Nome,
                dto.Sobrenome,
                email,
                celular,
                cpf,
                dataNascimento,
                dto.PerfilDeRiscoId
            );

            var response = _mapper.Map<ClienteResponseDto>(cliente);
            return Ok(response);
        }

        // GET: api/cliente/{id}
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var cliente = await _useCase.ObterPorIdAsync(id);
            if (cliente == null) return NotFound();

            var response = _mapper.Map<ClienteResponseDto>(cliente);
            return Ok(response);
        }

        // GET: api/cliente
        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetAll()
        {
            var clientes = await _useCase.ListarTodosAsync();
            var response = _mapper.Map<IEnumerable<ClienteResponseDto>>(clientes);
            return Ok(response);
        }

        // PUT: api/cliente/{id}/perfil
        [HttpPut("{id:int}/perfil")]
        public async Task<IActionResult> AtualizarPerfil(int id, [FromBody] PerfilDeRiscoRequestDto dto)
        {
            var clienteAtualizado = await _useCase.AtualizarPerfilAsync(id, dto.Id);
            if (clienteAtualizado == null) return NotFound();

            var response = _mapper.Map<ClienteResponseDto>(clienteAtualizado);
            return Ok(response);
        }

        // DELETE: api/cliente/{id}
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var removido = await _useCase.RemoverAsync(id);
            if (!removido) return NotFound();

            return NoContent();
        }



        // ✅ Adicionar Endereço
        [HttpPost("{clienteId}/enderecos")]
        public async Task<IActionResult> AdicionarEndereco(int clienteId, [FromBody] EnderecoRequest dto)
        {
            // Usa AutoMapper para converter DTO → Entidade
            var endereco = _mapper.Map<Endereco>(dto);
            endereco = new Endereco(
                endereco.Logradouro,
                endereco.Numero,
                endereco.Complemento,
                endereco.Bairro,
                endereco.Cidade,
                endereco.Estado,
                endereco.Cep,
                clienteId
            );

            var cliente = await _useCase.AdicionarEnderecoAsync(clienteId, endereco);
            if (cliente == null) return NotFound("Cliente não encontrado.");

            // Retorna DTO de resposta
            var response = _mapper.Map<ClienteResponse>(cliente);
            return Ok(response);
        }

        // ✅ Atualizar Endereço
        [HttpPut("{clienteId}/enderecos/{enderecoId}")]
        public async Task<IActionResult> AtualizarEndereco(int clienteId, int enderecoId, [FromBody] EnderecoRequest dto)
        {
            var cliente = await _useCase.AtualizarEnderecoAsync(
                clienteId,
                enderecoId,
                dto.Logradouro,
                dto.Numero,
                dto.Complemento,
                dto.Bairro,
                dto.Cidade,
                dto.Estado,
                dto.Cep
            );

            if (cliente == null) return NotFound("Cliente ou endereço não encontrado.");

            var response = _mapper.Map<ClienteResponse>(cliente);
            return Ok(response);
        }

        // ✅ Remover Endereço
        [HttpDelete("{clienteId}/enderecos/{enderecoId}")]
        public async Task<IActionResult> RemoverEndereco(int clienteId, int enderecoId)
        {
            var cliente = await _useCase.RemoverEnderecoAsync(clienteId, enderecoId);
            if (cliente == null) return NotFound("Cliente ou endereço não encontrado.");

            var response = _mapper.Map<ClienteResponse>(cliente);
            return Ok(response);
        }
    }
}
