using Painel.Investimento.Domain.Repository.Abstract;

namespace Painel.Investimento.Domain.Models
{
    public class PerfilProduto : IAggregateRoot
    {
        public int PerfilDeRiscoId { get; private set; }
        public PerfilDeRisco PerfilDeRisco { get; private set; }

        public int ProdutoInvestimentoId { get; private set; }
        public ProdutoInvestimento ProdutoInvestimento { get; private set; }

        private PerfilProduto() { } // EF Core

        public PerfilProduto(int perfilDeRiscoId, int produtoInvestimentoId)
        {
            if (perfilDeRiscoId <= 0)
                throw new ArgumentException("Perfil de risco inválido.", nameof(perfilDeRiscoId));

            if (produtoInvestimentoId <= 0)
                throw new ArgumentException("Produto de investimento inválido.", nameof(produtoInvestimentoId));

            PerfilDeRiscoId = perfilDeRiscoId;
            ProdutoInvestimentoId = produtoInvestimentoId;
        }
    }
}
