using AutoMapper;
using Painel.Investimento.Domain.Dtos;
using Painel.Investimento.Domain.Models;

namespace Painel.Investimento.API.Mapper
{
    public class InvestimentoProfile : Profile
    {
        public InvestimentoProfile()
        {
            // Entidade -> DTO
            CreateMap<Investimentos, InvestimentoDto>()
                .ForMember(dest => dest.NomeCliente, opt => opt.MapFrom(src => src.Cliente != null ? src.Cliente.Nome : null))
                .ForMember(dest => dest.NomeProduto, opt => opt.MapFrom(src => src.ProdutoInvestimento != null ? src.ProdutoInvestimento.Nome : null));

            // DTO -> Entidade (para criação)
            CreateMap<CreateInvestimentoDto, Investimentos>();
            CreateMap<RetiradaInvestimentoDto, Investimentos>();
            CreateMap<Investimentos, RetiradaInvestimentoDto>();

        }
    }
}
