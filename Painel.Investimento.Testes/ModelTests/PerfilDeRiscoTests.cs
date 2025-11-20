using System;
using Xunit;
using Painel.Investimento.Domain.Models;

public class PerfilDeRiscoTests
{
    [Fact]
    public void Construtor_DeveCriarPerfilValido()
    {
        // Act
        var perfil = new PerfilDeRisco(
            id: 1,
            nome: "Conservador",
            pontuacaoMinima: 0,
            pontuacaoMaxima: 20,
            descricao: "Perfil de baixo risco"
        );

        // Assert
        Assert.Equal(1, perfil.Id);
        Assert.Equal("Conservador", perfil.Nome);
        Assert.Equal(0, perfil.PontuacaoMinima);
        Assert.Equal(20, perfil.PontuacaoMaxima);
        Assert.Equal("Perfil de baixo risco", perfil.Descricao);
    }

    [Theory]
    [InlineData(1, "", 0, 20, "Desc")]          // Nome vazio
    [InlineData(1, "Moderado", -1, 20, "Desc")] // Pontuação mínima negativa
    [InlineData(1, "Moderado", 10, 5, "Desc")]  // Máxima menor ou igual à mínima
    [InlineData(1, "Moderado", 0, 20, "")]      // Descrição vazia
    public void AtualizarPerfil_DeveLancarExcecao_QuandoParametrosInvalidos(
        int id, string nome, int min, int max, string descricao)
    {
        // Arrange
        var perfil = new PerfilDeRisco(1, "Teste", 0, 10, "Desc");

        // Act & Assert
        Assert.Throws<ArgumentException>(() =>
            perfil.AtualizarPerfil(id, nome, min, max, descricao));
    }

    [Fact]
    public void PertenceAoIntervalo_DeveRetornarTrue_QuandoPontuacaoDentroDoIntervalo()
    {
        // Arrange
        var perfil = new PerfilDeRisco(1, "Moderado", 10, 30, "Perfil médio");

        // Act
        var resultado = perfil.PertenceAoIntervalo(20);

        // Assert
        Assert.True(resultado);
    }

    [Fact]
    public void PertenceAoIntervalo_DeveRetornarFalse_QuandoPontuacaoForaDoIntervalo()
    {
        // Arrange
        var perfil = new PerfilDeRisco(1, "Moderado", 10, 30, "Perfil médio");

        // Act
        var resultado = perfil.PertenceAoIntervalo(5);

        // Assert
        Assert.False(resultado);
    }

    [Fact]
    public void Validar_DeveLancarExcecao_QuandoPerfilInvalido()
    {
        // Arrange: cria perfil válido
        var perfil = new PerfilDeRisco(1, "Moderado", 0, 10, "Desc");

        // Manipula para deixar inválido (sem passar pelo AtualizarPerfil)
        typeof(PerfilDeRisco).GetProperty("Nome")!
            .SetValue(perfil, "");

        // Act & Assert: agora sim, a exceção vem do Validar()
        Assert.Throws<InvalidOperationException>(() => perfil.Validar());
    }


    [Fact]
    public void Validar_DevePassar_QuandoPerfilValido()
    {
        // Arrange
        var perfil = new PerfilDeRisco(1, "Moderado", 0, 10, "Desc");

        // Act & Assert
        perfil.Validar(); // não deve lançar exceção
    }
}
