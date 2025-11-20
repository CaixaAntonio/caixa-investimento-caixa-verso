using Painel.Investimento.Domain.Models;
using System;
using Xunit;

namespace Painel.Investimento.Testes.ModelTests
{
    public class ProdutoInvestimentoTests
    {
        [Fact]
        public void Deve_Criar_ProdutoInvestimento_Valido()
        {
            // Arrange & Act
            var produto = new ProdutoInvestimento(
                "CDB",
                "Renda Fixa",
                0.12m,
                10,
                "Diária",
                "IR 15%",
                "FGC",
                "Certificado de Depósito Bancário"
            );

            // Assert
            Assert.Equal("CDB", produto.Nome);
            Assert.Equal("Renda Fixa", produto.Tipo);
            Assert.Equal(0.12m, produto.RentabilidadeAnual);
            Assert.Equal(10, produto.Risco);
            Assert.Equal("Diária", produto.Liquidez);
            Assert.Equal("IR 15%", produto.Tributacao);
            Assert.Equal("FGC", produto.Garantia);
            Assert.Equal("Certificado de Depósito Bancário", produto.Descricao);
        }

        [Theory]
        [InlineData("", "Renda Fixa", 0.12, 10, "Diária", "IR 15%", "FGC", "Descrição válida", "Nome não pode ser vazio.")]
        [InlineData("CDB", "", 0.12, 10, "Diária", "IR 15%", "FGC", "Descrição válida", "Tipo de perfil não pode ser vazio.")]
        [InlineData("CDB", "Renda Fixa", 0, 10, "Diária", "IR 15%", "FGC", "Descrição válida", "Rentabilidade deve ser maior que zero.")]
        [InlineData("CDB", "Renda Fixa", 0.12, 0, "Diária", "IR 15%", "FGC", "Descrição válida", "Risco deve estar entre 1 e 100.")]
        [InlineData("CDB", "Renda Fixa", 0.12, 10, "", "IR 15%", "FGC", "Descrição válida", "Liquidez não pode ser vazia.")]
        [InlineData("CDB", "Renda Fixa", 0.12, 10, "Diária", "", "FGC", "Descrição válida", "Tributação não pode ser vazia.")]
        [InlineData("CDB", "Renda Fixa", 0.12, 10, "Diária", "IR 15%", "", "Descrição válida", "Garantia não pode ser vazia.")]
        [InlineData("CDB", "Renda Fixa", 0.12, 10, "Diária", "IR 15%", "FGC", "", "Descrição não pode ser vazia.")]
        public void Deve_Lancar_Excecao_Quando_Dados_Invalidos(
            string nome, string tipoPerfil, decimal rentabilidade, int risco,
            string liquidez, string tributacao, string garantia, string descricao,
            string mensagemEsperada)
        {
            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() =>
                new ProdutoInvestimento(nome, tipoPerfil, rentabilidade, risco, liquidez, tributacao, garantia, descricao));

            Assert.Contains(mensagemEsperada, ex.Message);
        }

        [Fact]
        public void Deve_Atualizar_ProdutoInvestimento_Com_Dados_Validos()
        {
            // Arrange
            var produto = new ProdutoInvestimento("CDB", "Renda Fixa", 0.12m, 10, "Diária", "IR 15%", "FGC", "Descrição inicial");

            // Act
            produto.AtualizarProdutoInvestimento("LCI", "Renda Fixa", 0.15m, 20, "Mensal", "Isento", "Sem Garantia", "Nova descrição");

            // Assert
            Assert.Equal("LCI", produto.Nome);
            Assert.Equal("Renda Fixa", produto.Tipo);
            Assert.Equal(0.15m, produto.RentabilidadeAnual);
            Assert.Equal(20, produto.Risco);
            Assert.Equal("Mensal", produto.Liquidez);
            Assert.Equal("Isento", produto.Tributacao);
            Assert.Equal("Sem Garantia", produto.Garantia);
            Assert.Equal("Nova descrição", produto.Descricao);
        }

        [Fact]
        public void Deve_Lancar_Excecao_Ao_Atualizar_Com_Dados_Invalidos()
        {
            // Arrange
            var produto = new ProdutoInvestimento("CDB", "Renda Fixa", 0.12m, 10, "Diária", "IR 15%", "FGC", "Descrição inicial");

            // Act & Assert
            Assert.Throws<ArgumentException>(() =>
                produto.AtualizarProdutoInvestimento("", "Renda Fixa", 0.12m, 10, "Diária", "IR 15%", "FGC", "Descrição válida"));
        }

        [Fact]
        public void Deve_Validar_Produto_Correto()
        {
            // Arrange
            var produto = new ProdutoInvestimento("CDB", "Renda Fixa", 0.12m, 10, "Diária", "IR 15%", "FGC", "Descrição válida");

            // Act & Assert
            produto.Validar(); // não deve lançar exceção
        }

        [Fact]
        public void Deve_Lancar_Excecao_Ao_Atualizar_Com_Rentabilidade_Invalida()
        {
            var produto = new ProdutoInvestimento("CDB", "Renda Fixa", 0.12m, 10, "Diária", "IR 15%", "FGC", "Descrição válida");

            Assert.Throws<ArgumentException>(() =>
                produto.AtualizarProdutoInvestimento("CDB", "Renda Fixa", -1m, 10, "Diária", "IR 15%", "FGC", "Descrição válida"));
        }
    }
}
