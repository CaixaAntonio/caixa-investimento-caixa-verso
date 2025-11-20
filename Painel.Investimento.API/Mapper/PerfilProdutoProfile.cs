using AutoMapper;
using Painel.Investimento.Domain.Models;
using Painel.Investimento.Domain.Dtos;

namespace Painel.Investimento.API.Mapper
{
    public class PerfilProdutoProfile : Profile
    {
        public PerfilProdutoProfile()
        {
            // ✅ Request → Entidade
            CreateMap<PerfilProdutoRequestDto, PerfilProduto>();

            // ✅ Entidade → Response
            CreateMap<PerfilProduto, PerfilProdutoResponseDto>()
                .ForMember(dest => dest.NomePerfilDeRisco,
                           opt => opt.MapFrom(src => src.PerfilDeRisco != null ? src.PerfilDeRisco.Nome : string.Empty))
                .ForMember(dest => dest.NomeProdutoInvestimento,
                           opt => opt.MapFrom(src => src.ProdutoInvestimento != null ? src.ProdutoInvestimento.Nome : string.Empty));
        }
    }
}
