using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Painel.investimento.Infra.Data;
using Painel.Investimento.Domain.Dtos;

namespace Painel.Investimento.API.Controllers
{
    [ApiController]
    [Route("api/usuarios")]
    public class UsuarioController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public UsuarioController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UsuarioDto>> GetById(int id)
        {
            try
            {
                var usuario = await _context.Usuarios.FindAsync(id);
                if (usuario == null)
                    return NotFound("Usuário não encontrado.");

                var dto = _mapper.Map<UsuarioDto>(usuario);
                return Ok(dto);
            }
            catch (ArgumentException ex)
            {
                // Exemplo: se o parâmetro id for inválido
                return BadRequest($"Parâmetro inválido: {ex.Message}");
            }
            catch (Exception ex)
            {
                // Captura qualquer erro inesperado
                return StatusCode(500, $"Erro ao buscar usuário: {ex.Message}");
            }
        }
    }
}
