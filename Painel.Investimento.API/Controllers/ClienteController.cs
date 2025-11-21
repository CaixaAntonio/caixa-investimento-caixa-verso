using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Painel.Investimento.Aplication.UseCaseInvestimentos;
using Painel.Investimento.Aplication.UseCasesCadastros;
using Painel.Investimento.Domain.Dtos;
using Painel.Investimento.Domain.Models;
using Painel.Investimento.Domain.Repository.Abstract;
using Painel.Investimento.Domain.Valueobjects;

[ApiController]
[Route("/[controller]")]
public class ClienteController : ControllerBase
{
    private readonly ClienteUseCase _useCase;
    private readonly IInvestimentosRepository _investimentoRepo;
    private readonly CalcularPerfilDeRiscoUseCase _calcularPerfilDeRisco;
    private readonly IMapper _mapper;

    public ClienteController(
        ClienteUseCase useCase,
        IInvestimentosRepository investimentoRepo,
        CalcularPerfilDeRiscoUseCase calcularPerfilDeRisco,
        IMapper mapper)
    {
        _useCase = useCase;
        _investimentoRepo = investimentoRepo;
        _calcularPerfilDeRisco = calcularPerfilDeRisco;
        _mapper = mapper;
    }

    [HttpGet("/perfil-risco/{clienteId:int}")]
    [Authorize]
    public async Task<ActionResult<object>> GetPerfilRisco(int clienteId)
    {
        try
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
        catch (Exception ex)
        {
            return StatusCode(500, $"Erro ao calcular perfil de risco: {ex.Message}");
        }
    }

    [HttpPost("registrar")]
    [Authorize]
    public async Task<IActionResult> Post([FromBody] ClienteRequestDto dto)
    {
        try
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
        catch (Exception ex)
        {
            return StatusCode(500, $"Erro ao cadastrar cliente: {ex.Message}");
        }
    }

    [HttpGet("cliente/{clientId:int}")]
    [Authorize]
    public async Task<IActionResult> GetById(int clientId)
    {
        try
        {
            var cliente = await _useCase.ObterPorIdAsync(clientId);
            if (cliente == null) return NotFound();

            var response = _mapper.Map<ClienteResponseDto>(cliente);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Erro ao buscar cliente: {ex.Message}");
        }
    }

    [HttpPut("perfil/{clientId:int}")]
    [Authorize]
    public async Task<IActionResult> AtualizarPerfil(int clientId, [FromBody] PerfilDeRiscoRequestDto dto)
    {
        try
        {
            var clienteAtualizado = await _useCase.AtualizarPerfilAsync(clientId, dto.Id);
            if (clienteAtualizado == null) return NotFound();

            var response = _mapper.Map<ClienteResponseDto>(clienteAtualizado);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Erro ao atualizar perfil: {ex.Message}");
        }
    }

    [HttpDelete("cliente/{clientId:int}")]
    [Authorize]
    public async Task<IActionResult> Delete(int clientId)
    {
        try
        {
            var removido = await _useCase.RemoverAsync(clientId);
            if (!removido) return NotFound();

            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Erro ao remover cliente: {ex.Message}");
        }
    }

    // Exemplo para Endereço
    [HttpPost("enderecos/{clienteId}")]
    [Authorize]
    public async Task<IActionResult> AdicionarEndereco(int clienteId, [FromBody] EnderecoRequest dto)
    {
        try
        {
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

            var response = _mapper.Map<ClienteResponse>(cliente);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Erro ao adicionar endereço: {ex.Message}");
        }
    }
}
