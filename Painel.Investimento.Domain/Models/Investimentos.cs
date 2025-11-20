namespace Painel.Investimento.Domain.Models
{
    public class Investimentos
    {
        public int Id { get; private set; }

        public int? ClienteId { get; private set; }
        public Cliente? Cliente { get; private set; }
        public int ProdutoInvestimentoId { get; private set; }
        public ProdutoInvestimento? ProdutoInvestimento { get; private set; }
        public decimal? ValorInvestido { get; private set; }
        public DateTime? DataInvestimento { get; private set; }
        public int? PrazoMeses { get; private set; }
        public bool Crise { get; private set; }
        public decimal? ValorRetirado { get; private set; }

        private Investimentos() { }

        public Investimentos(int clienteId, int produtoInvestimentoId, decimal valorInvestido, DateTime dataInvestimento,
                     int? prazoMeses)
        {
            if (produtoInvestimentoId <= 0)
                throw new ArgumentException("Produto de investimento inválido.", nameof(produtoInvestimentoId));

            if (valorInvestido <= 0)
                throw new ArgumentException("Valor investido deve ser maior que zero.", nameof(valorInvestido));

            ClienteId = clienteId;
            ProdutoInvestimentoId = produtoInvestimentoId;
            ValorInvestido = valorInvestido;
            DataInvestimento = dataInvestimento;
            PrazoMeses = prazoMeses;

        }
        public Investimentos(int clienteId, int produtoInvestimentoId, decimal valorInvestido, int? prazoMeses, DateTime dataInvestimento,
                     bool crise = false, decimal? valorRetirado = null)
        {
            if (produtoInvestimentoId <= 0)
                throw new ArgumentException("Produto de investimento inválido.", nameof(produtoInvestimentoId));

            if (valorRetirado <= 0)
                throw new ArgumentException("Valor investido deve ser maior que zero.", nameof(valorRetirado));

            ClienteId = clienteId;
            ProdutoInvestimentoId = produtoInvestimentoId;
            ValorInvestido = valorInvestido;
            DataInvestimento = dataInvestimento;
            PrazoMeses = prazoMeses;
            Crise = crise;
            ValorRetirado = valorRetirado;
        }

        public decimal CalcularValorFinal()
        {
            var valorBase = ValorInvestido ?? 0;

            if (ValorRetirado.HasValue)
                valorBase -= ValorRetirado.Value;

            if (Crise)
                valorBase *= 0.9m; // exemplo: reduzir 10% em caso de crise

            return valorBase;
        }

    }
}
