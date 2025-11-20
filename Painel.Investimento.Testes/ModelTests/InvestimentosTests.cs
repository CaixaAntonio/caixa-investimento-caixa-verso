using System;
using Xunit;
using Painel.Investimento.Domain.Models;

public class InvestimentosTests
{
    [Fact]
    public void Construtor_DeveCriarInvestimentoValido()
    {
        // Act
        var investimento = new Investimentos(
            clienteId: 1,
            produtoInvestimentoId: 10,
            valorInvestido: 1000,
            dataInvestimento: DateTime.UtcNow,
            prazoMeses: 12
        );

        // Assert
        Assert.Equal(1, investimento.ClienteId);
        Assert.Equal(10, investimento.ProdutoInvestimentoId);
        Assert.Equal(1000, investimento.ValorInvestido);
        Assert.Equal(12, investimento.PrazoMeses);
        Assert.NotNull(investimento.DataInvestimento);
    }

    [Theory]
    [InlineData(1, 0, 1000)]   // Produto inválido
    [InlineData(1, 10, 0)]     // Valor investido inválido
    public void Construtor_DeveLancarExcecao_QuandoParametrosInvalidos(int clienteId, int produtoId, decimal valor)
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() =>
            new Investimentos(clienteId, produtoId, valor, DateTime.UtcNow, 12));
    }

    [Fact]
    public void ConstrutorComRetirada_DeveCriarInvestimentoValido()
    {
        // Act
        var investimento = new Investimentos(
            clienteId: 1,
            produtoInvestimentoId: 10,
            valorInvestido: 1000,
            prazoMeses: 12,
            dataInvestimento: DateTime.UtcNow,
            crise: false,
            valorRetirado: 100
        );

        // Assert
        Assert.Equal(1, investimento.ClienteId);
        Assert.Equal(10, investimento.ProdutoInvestimentoId);
        Assert.Equal(1000, investimento.ValorInvestido);
        Assert.Equal(100, investimento.ValorRetirado);
        Assert.False(investimento.Crise);
    }

    [Fact]
    public void ConstrutorComRetirada_DeveLancarExcecao_QuandoValorRetiradoInvalido()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() =>
            new Investimentos(1, 10, 1000, 12, DateTime.UtcNow, false, 0));
    }

    [Fact]
    public void CalcularValorFinal_DeveRetornarValorSemRetiradaNemCrise()
    {
        // Arrange
        var investimento = new Investimentos(1, 10, 1000, DateTime.UtcNow, 12);

        // Act
        var valorFinal = investimento.CalcularValorFinal();

        // Assert
        Assert.Equal(1000, valorFinal);
    }

    [Fact]
    public void CalcularValorFinal_DeveRetornarValorComRetirada()
    {
        // Arrange
        var investimento = new Investimentos(1, 10, 1000, 12, DateTime.UtcNow, false, 200);

        // Act
        var valorFinal = investimento.CalcularValorFinal();

        // Assert
        Assert.Equal(800, valorFinal);
    }

    [Fact]
    public void CalcularValorFinal_DeveRetornarValorComCrise()
    {
        // Arrange
        var investimento = new Investimentos(1, 10, 1000, 12, DateTime.UtcNow, true, null);

        // Act
        var valorFinal = investimento.CalcularValorFinal();

        // Assert
        Assert.Equal(900, valorFinal); // 10% de redução
    }

    [Fact]
    public void CalcularValorFinal_DeveRetornarValorComRetiradaEComCrise()
    {
        // Arrange
        var investimento = new Investimentos(1, 10, 1000, 12, DateTime.UtcNow, true, 200);

        // Act
        var valorFinal = investimento.CalcularValorFinal();

        // Assert
        Assert.Equal(720, valorFinal); // (1000 - 200) * 0.9
    }
}
