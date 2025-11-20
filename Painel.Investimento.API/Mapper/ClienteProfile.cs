using AutoMapper;
using Painel.Investimento.API.Mapper;
using Painel.Investimento.Domain.Dtos;
using Painel.Investimento.Domain.Models;
using Painel.Investimento.Domain.Valueobjects;

namespace Painel.investimento.API.Mapper
{
    public class ClienteProfile : Profile
    {
        public ClienteProfile()
        {
            CreateMap<EnderecoRequest, Endereco>();
            CreateMap<Endereco, EnderecoResponse>();


            // Registro no profile:
            CreateMap<DataDeNascimento, DateTime>().ConvertUsing<DataDeNascimentoToDateTimeConverter>();

                // Entidade → Response DTO
             CreateMap<Cliente, ClienteResponseDto>()
                .ForMember(dest => dest.NomeCompleto, opt => opt.MapFrom(src => $"{src.Nome} {src.Sobrenome}"))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email.Value))
                .ForMember(dest => dest.Celular, opt => opt.MapFrom(src => src.Celular.Numero))
                .ForMember(dest => dest.Cpf, opt => opt.MapFrom(src => src.Cpf.Numero))
                .ForMember(dest => dest.Idade, opt => opt.MapFrom(src => src.Idade.Valor))
                .ForMember(dest => dest.DataDeNascimento, opt => opt.MapFrom(src => src.DataDeNascimento.Valor))
                .ForMember(dest => dest.PerfilDeRiscoId, opt => opt.MapFrom(src => src.PerfilDeRiscoId));
               

            // Request DTO → Entidade (parcial, pois ValueObjects são criados no Controller)
            CreateMap<ClienteRequestDto, Cliente>()
                .ForMember(dest => dest.Nome, opt => opt.MapFrom(src => src.Nome))
                .ForMember(dest => dest.Sobrenome, opt => opt.MapFrom(src => src.Sobrenome));

           

            CreateMap<Cliente, ClienteResponse>();
            CreateMap<ClienteRequestDto, Cliente>();
        }
    }
}
