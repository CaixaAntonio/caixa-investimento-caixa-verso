namespace Painel.Investimento.Domain.Dtos
{
    public class InvestimentoDto
    {
        public int Id { get; set; }
        public int ClienteId { get; set; }
        public string? NomeCliente { get; set; }
        public int ProdutoInvestimentoId { get; set; }
        public string? NomeProduto { get; set; }
        public decimal? ValorInvestido { get; set; }
        public DateTime? DataInvestimento { get; set; }
        public int? PrazoMeses { get; set; }
        public bool Crise { get; set; }
        public decimal? ValorRetirado { get; set; }
    }

    public class CreateInvestimentoDto
    {
        public int ClienteId { get; set; }
        public int ProdutoInvestimentoId { get; set; }
        public decimal ValorInvestido { get; set; }
        public DateTime DataInvestimento { get; set; }
        public int? PrazoMeses { get; set; }
        public bool Crise { get; set; }
        public decimal? ValorRetirado { get; set; }
    }

    public class RetiradaInvestimentoDto
    {
        public int ClienteId { get; set; }
        public int ProdutoInvestimentoId { get; set; }
        public decimal ValorInvestido { get; set; }
        public DateTime DataInvestimento { get; set; }
        public int? PrazoMeses { get; set; }
        public bool Crise { get; set; }
        public decimal? ValorRetirado { get; set; }
    }

    public class InvestimentoResumoDto
    {
        public int Id { get; set; }
        public string Tipo { get; set; } = string.Empty;
        public decimal Valor { get; set; }
        public decimal Rentabilidade { get; set; }
        public DateTime Data { get; set; }
    }

}
