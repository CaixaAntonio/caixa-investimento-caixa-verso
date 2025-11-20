namespace Painel.Investimento.Domain.Dtos
{
    // DTO para entrada de dados (Request)
    public class PerfilProdutoRequestDto
    {
        public int PerfilDeRiscoId { get; set; }          // Id do perfil de risco
        public int ProdutoInvestimentoId { get; set; }    // Id do produto de investimento
    }

    // DTO para saída de dados (Response)
    public class PerfilProdutoResponseDto
    {
        public int PerfilDeRiscoId { get; set; }
        public string? NomePerfilDeRisco { get; set; }    // Nome do perfil de risco

        public int ProdutoInvestimentoId { get; set; }
        public string? NomeProdutoInvestimento { get; set; } // Nome do produto de investimento
    }
}
