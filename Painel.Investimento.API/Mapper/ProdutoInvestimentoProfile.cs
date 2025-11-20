using AutoMapper;
using Painel.Investimento.Domain.Dtos;
using Painel.Investimento.Domain.Models;

namespace Painel.Investimento.API.Mapper
{
    public class ProdutoInvestimentoProfile : Profile
    {
        public ProdutoInvestimentoProfile()
        {
            // Request → Domain
            CreateMap<ProdutoInvestimentoRequestDto, ProdutoInvestimento>()
                .ConstructUsing(dto => new ProdutoInvestimento(
                    dto.Nome!,
                    dto.Tipo!,
                    dto.RentabilidadeAnual,
                    dto.Risco,
                    dto.Liquidez!,
                    dto.Tributacao!,
                    dto.Garantia!,
                    dto.Descricao!
                ));

            // Domain → Response
            CreateMap<ProdutoInvestimento, ProdutoInvestimentoResponseDto>()
                .ForMember(dest => dest.Tipo, opt => opt.MapFrom(src => src.Tipo))
                .ForMember(dest => dest.Liquidez, opt => opt.MapFrom(src => src.Liquidez))
                .ForMember(dest => dest.Tributacao, opt => opt.MapFrom(src => src.Tributacao))
                .ForMember(dest => dest.Garantia, opt => opt.MapFrom(src => src.Garantia))
                .ForMember(dest => dest.Descricao, opt => opt.MapFrom(src => src.Descricao));
        }
    }
}
