using System;
using Xunit;
using Painel.Investimento.Domain.Models;

public class PerfilProdutoTests
{
    [Fact]
    public void Construtor_DeveCriarPerfilProdutoValido()
    {
        // Act
        var perfilProduto = new PerfilProduto(
            perfilDeRiscoId: 1,
            produtoInvestimentoId: 10
        );

        // Assert
        Assert.Equal(1, perfilProduto.PerfilDeRiscoId);
        Assert.Equal(10, perfilProduto.ProdutoInvestimentoId);
    }

    [Fact]
    public void Construtor_DeveLancarExcecao_QuandoPerfilDeRiscoIdInvalido()
    {
        // Act & Assert
        var ex = Assert.Throws<ArgumentException>(() =>
            new PerfilProduto(perfilDeRiscoId: 0, produtoInvestimentoId: 10));

        Assert.Contains("Perfil de risco inválido", ex.Message);
    }

    [Fact]
    public void Construtor_DeveLancarExcecao_QuandoProdutoInvestimentoIdInvalido()
    {
        // Act & Assert
        var ex = Assert.Throws<ArgumentException>(() =>
            new PerfilProduto(perfilDeRiscoId: 1, produtoInvestimentoId: 0));

        Assert.Contains("Produto de investimento inválido", ex.Message);
    }

    [Theory]
    [InlineData(-1, 10)]   // Perfil de risco negativo
    [InlineData(1, -5)]    // Produto investimento negativo
    public void Construtor_DeveLancarExcecao_QuandoIdsNegativos(int perfilId, int produtoId)
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() =>
            new PerfilProduto(perfilId, produtoId));
    }
}
