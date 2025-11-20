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
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null) return NotFound();

            return Ok(_mapper.Map<UsuarioDto>(usuario));
        }
    }
}
