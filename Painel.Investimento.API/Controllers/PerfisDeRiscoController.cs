using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Painel.Investimento.Domain.Repository.Abstract;

namespace Painel.Investimento.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PerfisDeRiscoController : ControllerBase
    {
        private readonly IPerfilDeRiscoRepository _perfilRepo;
        private readonly IMapper _mapper;

        // ✅ Apenas um construtor
        public PerfisDeRiscoController(IPerfilDeRiscoRepository perfilRepo, IMapper mapper)
        {
            _perfilRepo = perfilRepo;
            _mapper = mapper;
        }

        /// <summary>
        /// Retorna todos os perfis de risco cadastrados.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PerfilDeRiscoDto>>> GetPerfis()
        {
            var perfis = await _perfilRepo.GetAllAsync();

            // ✅ Mapeia lista para lista
            var dtoList = _mapper.Map<IEnumerable<PerfilDeRiscoDto>>(perfis);

            return Ok(dtoList);
        }

        /// <summary>
        /// Retorna um perfil de risco específico pelo Id.
        /// </summary>
        [HttpGet("{PerfiDeRiscoId:int}")]
        public async Task<ActionResult<PerfilDeRiscoDto>> GetPerfil(int id)
        {
            var perfil = await _perfilRepo.GetByIdAsync(id);
            if (perfil == null)
                return NotFound();

            // ✅ Mapeia objeto único
            var dto = _mapper.Map<PerfilDeRiscoDto>(perfil);

            return Ok(dto);
        }
    }
}
