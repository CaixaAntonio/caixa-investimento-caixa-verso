using AutoMapper;
using Painel.Investimento.Domain.Valueobjects;

namespace Painel.Investimento.API.Mapper
{
    public class DataDeNascimentoToDateTimeConverter : ITypeConverter<DataDeNascimento, DateTime>
    {
        public DateTime Convert(DataDeNascimento source, DateTime destination, ResolutionContext context)
            => source?.Valor ?? default;
    }
}

