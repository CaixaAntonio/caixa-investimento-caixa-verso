using System;
using Xunit;
using Painel.Investimento.Domain.Models;

public class SimulacaoTests
{
    [Fact]
    public void Construtor_DeveCriarSimulacaoValida()
    {
        // Arrange & Act
        var simulacao = new Simulacao(
            clienteId: 1,
            nomeProduto: "CDB",
            valorInicial: 1000,
            prazoMeses: 12,
            valorFinal: 1200
        );

        // Assert
        Assert.Equal(1, simulacao.ClienteId);
        Assert.Equal("CDB", simulacao.NomeProduto);
        Assert.Equal(1000, simulacao.ValorInicial);
        Assert.Equal(1200, simulacao.ValorFinal);
        Assert.Equal(12, simulacao.PrazoMeses);
        Assert.True(simulacao.RentabilidadeEfetiva > 0);
        Assert.True(simulacao.DataSimulacao <= DateTime.UtcNow);
    }

    [Theory]
    [InlineData(0, "CDB", 1000, 12, 1200)]   // ClienteId inválido
    [InlineData(1, "", 1000, 12, 1200)]      // NomeProduto vazio
    [InlineData(1, "CDB", 0, 12, 1200)]      // ValorInicial inválido
    [InlineData(1, "CDB", 1000, 0, 1200)]    // PrazoMeses inválido
    public void Construtor_DeveLancarExcecao_QuandoParametrosInvalidos(
        int clienteId, string nomeProduto, decimal valorInicial, int prazoMeses, decimal valorFinal)
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() =>
            new Simulacao(clienteId, nomeProduto, valorInicial, prazoMeses, valorFinal));
    }

    [Fact]
    public void CalcularRentabilidadePercentual_DeveRetornarCorreto()
    {
        // Arrange
        var simulacao = new Simulacao(1, "CDB", 1000, 12, 1200);

        // Act
        var percentual = simulacao.CalcularRentabilidadePercentual();

        // Assert
        // RentabilidadeEfetiva = ((1200 - 1000) / 1000) * 100 = 20 (%)
        // Percentual = (20 / 1000) * 100 = 2
        Assert.Equal(2, percentual);
    }

    [Fact]
    public void EhRentavel_DeveRetornarTrue_QuandoRentabilidadeMaiorQueMinimo()
    {
        // Arrange
        var simulacao = new Simulacao(1, "CDB", 1000, 12, 1200);

        // Act
        var resultado = simulacao.EhRentavel(1); // mínimo 1%

        // Assert
        Assert.True(resultado);
    }

    [Fact]
    public void EhRentavel_DeveRetornarFalse_QuandoRentabilidadeMenorQueMinimo()
    {
        // Arrange
        var simulacao = new Simulacao(1, "CDB", 1000, 12, 1010);

        // Act
        var resultado = simulacao.EhRentavel(5); // mínimo 5%

        // Assert
        Assert.False(resultado);
    }

    [Fact]
    public void Validar_DeveLancarExcecao_QuandoSimulacaoInvalida()
    {
        // Arrange
        var simulacao = new Simulacao(1, "CDB", 1000, 12, 1200);

        // Manipula para deixar inválido
        var simulacaoInvalida = new Simulacao(1, "CDB", 1000, 12, 1200);
        typeof(Simulacao).GetProperty("NomeProduto")?.SetValue(simulacaoInvalida, "");

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => simulacaoInvalida.Validar());
    }
}
