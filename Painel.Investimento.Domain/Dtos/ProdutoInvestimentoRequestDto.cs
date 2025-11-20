namespace Painel.Investimento.Domain.Dtos
{
    // DTO para entrada de dados (Request)
    public class ProdutoInvestimentoRequestDto
    {
        public string? Nome { get; set; }                // Ex: "CDB", "LCI", "Tesouro Direto"
        public string? Tipo { get; set; }          // Perfil do produto (ex: Conservador, Moderado, Arrojado)
        public decimal RentabilidadeAnual { get; set; }  // Ex: 0.12 para 12% ao ano
        public int Risco { get; set; }                   // Escala de risco (1 a 100)
        public string? Liquidez { get; set; }            // Ex: "Diária", "Mensal"
        public string? Tributacao { get; set; }          // Ex: "IR 15%", "Isento"
        public string? Garantia { get; set; }            // Ex: "Fundo Garantidor de Créditos"
        public string? Descricao { get; set; }           // Texto explicativo
    }

    // DTO para saída de dados (Response)
    public class ProdutoInvestimentoResponseDto
    {
        public int Id { get; set; }
        public string? Nome { get; set; }
        public string? Tipo { get; set; }
        public decimal RentabilidadeAnual { get; set; }
        public int Risco { get; set; }
        public string? Liquidez { get; set; }
        public string? Tributacao { get; set; }
        public string? Garantia { get; set; }
        public string? Descricao { get; set; }
    }
}
