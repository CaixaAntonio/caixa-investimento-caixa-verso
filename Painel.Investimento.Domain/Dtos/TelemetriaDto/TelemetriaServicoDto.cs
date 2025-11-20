using System.Text.Json.Serialization;
using System.Text.Json;

namespace Painel.Investimento.Domain.Dtos.TelemetriaDto
{
    public class TelemetriaServicoDto
    {
        public string Nome { get; set; } = string.Empty;
        public int QuantidadeChamadas { get; set; }
        public double MediaTempoRespostaMs { get; set; }
    }

    public class TelemetriaPeriodoDto
    {
        [JsonConverter(typeof(DateOnlyConverter))]
        public DateTime Inicio { get; set; }

        [JsonConverter(typeof(DateOnlyConverter))]
        public DateTime Fim { get; set; }
    }

    public class TelemetriaResponse
    {
        public IEnumerable<TelemetriaServicoDto> Servicos { get; set; } = new List<TelemetriaServicoDto>();
        public TelemetriaPeriodoDto Periodo { get; set; } = new TelemetriaPeriodoDto();
    }


    public class DateOnlyConverter : JsonConverter<DateTime>
    {
        private const string Format = "yyyy-MM-dd";

        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            => DateTime.Parse(reader.GetString()!);

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
            => writer.WriteStringValue(value.ToString(Format));
    }

}
