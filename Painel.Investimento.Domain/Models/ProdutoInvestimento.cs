using System;
using Painel.Investimento.Domain.Repository.Abstract;

namespace Painel.Investimento.Domain.Models
{
    public class ProdutoInvestimento : IAggregateRoot
    {
        public int? Id { get; private set; }
        public string Nome { get; private set; }
        public string Tipo { get; private set; }
        public decimal RentabilidadeAnual { get; private set; }
        public int Risco { get; private set; } // ✅ mantido como int (escala 1–100)
        public string Liquidez { get; private set; }
        public string Tributacao { get; private set; }
        public string Garantia { get; private set; }
        public string Descricao { get; private set; }

        public ICollection<PerfilProduto> PerfilProdutos { get; private set; } = new List<PerfilProduto>();
        public ICollection<Investimentos> Investimentos { get; private set; }

        private ProdutoInvestimento() { } // EF Core

        public ProdutoInvestimento(string nome, string tipoPerfil, decimal rentabilidadeAnual, int risco,
                                   string liquidez, string tributacao, string garantia, string descricao)
        {
            AtualizarProdutoInvestimento(nome, tipoPerfil, rentabilidadeAnual, risco, liquidez, tributacao, garantia, descricao);
        }

        public void AtualizarProdutoInvestimento(string nome, string tipo, decimal rentabilidadeAnual, int risco,
                                                 string liquidez, string tributacao, string garantia, string descricao)
        {
            if (string.IsNullOrWhiteSpace(nome))
                throw new ArgumentException("Nome não pode ser vazio.");
            Nome = nome;

            if (string.IsNullOrWhiteSpace(tipo))
                throw new ArgumentException("Tipo de perfil não pode ser vazio.");
            Tipo = tipo;

            if (rentabilidadeAnual <= 0)
                throw new ArgumentException("Rentabilidade deve ser maior que zero.");
            RentabilidadeAnual = rentabilidadeAnual;

            if (risco < 1 || risco > 100)
                throw new ArgumentException("Risco deve estar entre 1 e 100.");
            Risco = risco;

            if (string.IsNullOrWhiteSpace(liquidez))
                throw new ArgumentException("Liquidez não pode ser vazia.");
            Liquidez = liquidez;

            if (string.IsNullOrWhiteSpace(tributacao))
                throw new ArgumentException("Tributação não pode ser vazia.");
            Tributacao = tributacao;

            if (string.IsNullOrWhiteSpace(garantia))
                throw new ArgumentException("Garantia não pode ser vazia.");
            Garantia = garantia;

            if (string.IsNullOrWhiteSpace(descricao))
                throw new ArgumentException("Descrição não pode ser vazia.");
            Descricao = descricao;
        }

        public void Validar()
        {
            if (string.IsNullOrWhiteSpace(Nome))
                throw new InvalidOperationException("Produto inválido: Nome obrigatório.");
            if (RentabilidadeAnual <= 0)
                throw new InvalidOperationException("Produto inválido: Rentabilidade deve ser maior que zero.");
            if (Risco < 1 || Risco > 100)
                throw new InvalidOperationException("Produto inválido: Risco deve estar entre 1 e 100.");
            if (string.IsNullOrWhiteSpace(Liquidez))
                throw new InvalidOperationException("Produto inválido: Liquidez obrigatória.");
            if (string.IsNullOrWhiteSpace(Tributacao))
                throw new InvalidOperationException("Produto inválido: Tributação obrigatória.");
            if (string.IsNullOrWhiteSpace(Garantia))
                throw new InvalidOperationException("Produto inválido: Garantia obrigatória.");
            if (string.IsNullOrWhiteSpace(Descricao))
                throw new InvalidOperationException("Produto inválido: Descrição obrigatória.");
        }
    }
}
