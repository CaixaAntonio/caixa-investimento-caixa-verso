using System.Text.Json;
using System.Text.Json.Serialization;

namespace Painel.Investimento.Domain.Dtos
{
    public class SimulacaoInvestimentoRequest
    {
        public int ClienteId { get; set; }

        [JsonPropertyName("Valor")]
        [JsonConverter(typeof(JsonDecimalConverter))]
        public decimal Valor { get; set; }
        public int PrazoMeses { get; set; }
        public string NomeDoProduto { get; set; }
    }

    public class ProdutoValidadoDto
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Tipo { get; set; }

        [JsonPropertyName("Rentabilidade")]
        [JsonConverter(typeof(JsonDecimalConverter))]
        public decimal Rentabilidade { get; set; }
        public string Risco { get; set; }
    }

    public class ResultadoSimulacaoDto
    {
        [JsonPropertyName("ValorFinal")]
        [JsonConverter(typeof(JsonDecimalConverter))]
        public decimal ValorFinal { get; set; }

        [JsonPropertyName("RentabilidadeEfetiva")]
        [JsonConverter(typeof(JsonDecimalConverter))]
        public decimal RentabilidadeEfetiva { get; set; }
        public int PrazoMeses { get; set; }
    }

    public class SimulacaoInvestimentoResponse
    {
        public ProdutoValidadoDto ProdutoValidado { get; set; }
        public ResultadoSimulacaoDto ResultadoSimulacao { get; set; }
        public DateTime DataSimulacao { get; set; }
    }

    public class SimulacaoHistoricoResponse
    {
        public int ClienteId { get; set; }
        public IEnumerable<SimulacaoInvestimentoResponse> Simulacoes { get; set; }
    }

    // DTO de resposta para o endpoint
    public class RentabilidadeResponse
    {
        public int SimulacaoId { get; set; }

        [JsonPropertyName("PercentualRentabilidade")]
        [JsonConverter(typeof(JsonDecimalConverter))]
        public decimal PercentualRentabilidade { get; set; }
        public bool EhRentavel { get; set; }

        [JsonPropertyName("ValorInicial")]
        [JsonConverter(typeof(JsonDecimalConverter))]
        public decimal ValorInicial { get; set; }

        [JsonPropertyName("ValorFinal")]
        [JsonConverter(typeof(JsonDecimalConverter))]
        public decimal ValorFinal { get; set; }

        [JsonPropertyName("RentabilidadeEfetiva")]
        [JsonConverter(typeof(JsonDecimalConverter))]
        public decimal RentabilidadeEfetiva { get; set; }
        public int PrazoMeses { get; set; }
        public DateTime DataSimulacao { get; set; }
    }

    public class SimulacaoResponse
    {
        public int Id { get; set; }
        public int ClienteId { get; set; }
        public string NomeProduto { get; set; } = string.Empty;
        public decimal ValorInicial { get; set; }
        public decimal ValorFinal { get; set; }
        public decimal RentabilidadeEfetiva { get; set; }
        public int PrazoMeses { get; set; }
        public DateTime DataSimulacao { get; set; }
    }
    public class SimulacaoPorDiaProdutoResponse
    {
        public string Produto { get; set; }
        public DateTime Data { get; set; }
        public int QuantidadeSimulacoes { get; set; }

        [JsonPropertyName("MediaValorFinal")]
        [JsonConverter(typeof(JsonDecimalConverter))]
        public decimal MediaValorFinal { get; set; }
    }
    public class SimulacaoResumoDto
    {
        public int Id { get; set; }
        public int ClienteId { get; set; }
        public string Produto { get; set; } = string.Empty;

        [JsonPropertyName("ValorInvestido")]
        [JsonConverter(typeof(JsonDecimalConverter))]
        public decimal ValorInvestido { get; set; }

        [JsonPropertyName("valorFinal")]
        [JsonConverter(typeof(JsonDecimalConverter))]
        public decimal ValorFinal { get; set; }
        public int PrazoMeses { get; set; }
        public DateTime DataSimulacao { get; set; }
    }


    public class JsonDecimalConverter : JsonConverter<decimal>
    {
        public override decimal Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            => reader.GetDecimal();

        public override void Write(Utf8JsonWriter writer, decimal value, JsonSerializerOptions options)
            => writer.WriteNumberValue(Math.Round(value, 2));
    }


}
