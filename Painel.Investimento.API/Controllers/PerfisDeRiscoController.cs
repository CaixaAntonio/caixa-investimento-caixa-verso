using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Painel.Investimento.Domain.Repository.Abstract;

namespace Painel.Investimento.API.Controllers
{
    [ApiController]
    [Route("/[controller]")]
    public class PerfisDeRiscoController : ControllerBase
    {
        private readonly IPerfilDeRiscoRepository _perfilRepo;
        private readonly IMapper _mapper;

        public PerfisDeRiscoController(IPerfilDeRiscoRepository perfilRepo, IMapper mapper)
        {
            _perfilRepo = perfilRepo;
            _mapper = mapper;
        }

        /// <summary>
        /// Retorna todos os perfis de risco cadastrados.
        /// </summary>
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<PerfilDeRiscoDto>>> GetPerfis()
        {
            try
            {
                var perfis = await _perfilRepo.GetAllAsync();
                var dtoList = _mapper.Map<IEnumerable<PerfilDeRiscoDto>>(perfis);
                return Ok(dtoList);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao listar perfis de risco: {ex.Message}");
            }
        }

        /// <summary>
        /// Retorna um perfil de risco específico pelo Id.
        /// </summary>
        [HttpGet("{PerfiDeRiscoId:int}")]
        [Authorize]
        public async Task<ActionResult<PerfilDeRiscoDto>> GetPerfil(int PerfiDeRiscoId)
        {
            try
            {
                var perfil = await _perfilRepo.GetByIdAsync(PerfiDeRiscoId);
                if (perfil == null)
                    return NotFound("Perfil de risco não encontrado.");

                var dto = _mapper.Map<PerfilDeRiscoDto>(perfil);
                return Ok(dto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao buscar perfil de risco: {ex.Message}");
            }
        }
    }
}
