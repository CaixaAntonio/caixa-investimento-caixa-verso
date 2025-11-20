using AutoMapper;
using Painel.Investimento.Domain.Dtos;

namespace Painel.Investimento.API.Mapper
{
    public class SimulacaoProfile : Profile
    {
        public SimulacaoProfile()
        {
            // Request → UseCase DTO
            CreateMap<SimulacaoInvestimentoRequest, SimulacaoInvestimentoRequest>();

            // UseCase Response → API Response
            CreateMap<SimulacaoInvestimentoResponse, SimulacaoInvestimentoResponse>();
            CreateMap<SimulacaoHistoricoResponse, SimulacaoHistoricoResponse>();
        }
    }
}
