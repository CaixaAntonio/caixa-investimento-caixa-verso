using AutoMapper;
using Painel.Investimento.Domain.Dtos;
using Painel.Investimento.Domain.Models;

namespace Painel.Investimento.API.Mapper
{
    public class UsuarioProfile : Profile
    {
        public UsuarioProfile()
        {
            // Entidade -> DTO
            CreateMap<Usuario, UsuarioDto>();

            // DTO -> Entidade (caso queira criar usuários via API)
            CreateMap<UsuarioDto, Usuario>()
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore());
            // ⚠️ nunca mapear senha diretamente, use serviço de hash
        }
    }
}
