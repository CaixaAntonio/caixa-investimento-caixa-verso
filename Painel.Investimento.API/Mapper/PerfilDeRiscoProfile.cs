using AutoMapper;
using Painel.Investimento.Domain.Models;

namespace Painel.Investimento.API.Mapper
{
    public class PerfilDeRiscoProfile : Profile
    {
        public PerfilDeRiscoProfile()
        {
            // PerfilDeRisco → PerfilDeRiscoDto
            CreateMap<PerfilDeRisco, PerfilDeRiscoDto>();

        }
    }
}
