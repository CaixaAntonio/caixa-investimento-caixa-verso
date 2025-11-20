using System;
using System.Collections.Generic;
using Xunit;
using Painel.Investimento.Domain.Models;
using Painel.Investimento.Domain.Valueobjects;

public class ClienteTests
{
    private Email CriarEmailValido() => new Email("teste@dominio.com");
    private Celular CriarCelularValido() => new Celular("31999999999");
    private Cpf CriarCpfValido() => new Cpf("12345678901");
    private DataDeNascimento CriarDataNascimentoValida() => new DataDeNascimento(new DateTime(1990, 1, 1));

    [Fact]
    public void Construtor_DeveCriarClienteValido()
    {
        // Act
        var cliente = new Cliente(
            id: 1,
            nome: "João",
            sobrenome: "Silva",
            email: CriarEmailValido(),
            celular: CriarCelularValido(),
            cpf: CriarCpfValido(),
            dataDeNascimento: CriarDataNascimentoValida(),
            perfilDeRiscoId: 2
        );

        // Assert
        Assert.Equal(1, cliente.Id);
        Assert.Equal("João", cliente.Nome);
        Assert.Equal("Silva", cliente.Sobrenome);
        Assert.NotNull(cliente.Email);
        Assert.NotNull(cliente.Celular);
        Assert.NotNull(cliente.Cpf);
        Assert.NotNull(cliente.DataDeNascimento);
        Assert.Equal(2, cliente.PerfilDeRiscoId);
        Assert.True(cliente.Idade.Valor > 0);
    }

    [Theory]
    [InlineData("", "Silva")] // Nome vazio
    [InlineData("João", "")]  // Sobrenome vazio
    public void Construtor_DeveLancarExcecao_QuandoNomeOuSobrenomeInvalidos(string nome, string sobrenome)
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() =>
            new Cliente(1, nome, sobrenome, CriarEmailValido(), CriarCelularValido(), CriarCpfValido(), CriarDataNascimentoValida(), 1));
    }

    [Fact]
    public void AdicionarInvestimento_DeveAdicionarInvestimento()
    {
        // Arrange
        var cliente = new Cliente(
            1, "João", "Silva",
            CriarEmailValido(),
            CriarCelularValido(),
            CriarCpfValido(),
            CriarDataNascimentoValida(),
            1
        );

        var investimento = new Investimentos(
            clienteId: cliente.Id,
            produtoInvestimentoId: 10, // ID do produto
            valorInvestido: 1000,
            dataInvestimento: DateTime.UtcNow,
            prazoMeses: 12
        );

        // Act
        cliente.AdicionarInvestimento(investimento);

        // Assert
        Assert.Single(cliente.Investimentos);
        Assert.Contains(investimento, cliente.Investimentos);
    }


    [Fact]
    public void AdicionarEndereco_DeveAdicionarEndereco()
    {
        // Arrange
        var cliente = new Cliente(1, "João", "Silva", CriarEmailValido(), CriarCelularValido(), CriarCpfValido(), CriarDataNascimentoValida(), 1);
        var endereco = new Endereco("Rua A", "100", "Ap 1", "Centro", "BH", "MG", "30000-000",1);

        // Act
        cliente.AdicionarEndereco(endereco);

        // Assert
        Assert.Single(cliente.Enderecos);
        Assert.Contains(endereco, cliente.Enderecos);
    }

    [Fact]
    public void AtualizarEndereco_DeveAtualizarEnderecoExistente()
    {
        // Arrange
        var cliente = new Cliente(1, "João", "Silva", CriarEmailValido(), CriarCelularValido(), CriarCpfValido(), CriarDataNascimentoValida(), 1);
        var endereco = new Endereco( "Rua A", "100", "Ap 1", "Centro", "BH", "MG", "30000-000",1);
        cliente.AdicionarEndereco(endereco);

        // Act
        cliente.AtualizarEndereco(endereco.Id, "Rua B", "200", "Casa", "Bairro Novo", "SP", "SP", "40000-000");

        // Assert
        var atualizado = Assert.Single(cliente.Enderecos);
        Assert.Equal("Rua B", atualizado.Logradouro);
        Assert.Equal("200", atualizado.Numero);
        Assert.Equal("Casa", atualizado.Complemento);
        Assert.Equal("Bairro Novo", atualizado.Bairro);
        Assert.Equal("SP", atualizado.Cidade);
        Assert.Equal("SP", atualizado.Estado);
        Assert.Equal("40000-000", atualizado.Cep);
    }

    [Fact]
    public void AtualizarEndereco_DeveLancarExcecao_QuandoEnderecoNaoExiste()
    {
        // Arrange
        var cliente = new Cliente(1, "João", "Silva", CriarEmailValido(), CriarCelularValido(), CriarCpfValido(), CriarDataNascimentoValida(), 1);

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() =>
            cliente.AtualizarEndereco(99, "Rua B", "200", "Casa", "Bairro Novo", "SP", "SP", "40000-000"));
    }
       

    [Fact]
    public void RemoverEndereco_DeveRemoverEnderecoExistente()
    {
        // Arrange
        var cliente = new Cliente(1, "João", "Silva", CriarEmailValido(), CriarCelularValido(), CriarCpfValido(), CriarDataNascimentoValida(), 1);
        var endereco = new Endereco("Rua A", "100", "Ap 1", "Centro", "BH", "MG", "30000-000", 1);
        cliente.AdicionarEndereco(endereco);

        // Act
        cliente.RemoverEndereco(endereco.Id); // Id = 0

        // Assert
        Assert.Empty(cliente.Enderecos);
    }


    [Fact]
    public void AjustarPerfil_DeveAlterarPerfilDeRisco()
    {
        // Arrange
        var cliente = new Cliente(1, "João", "Silva", CriarEmailValido(), CriarCelularValido(), CriarCpfValido(), CriarDataNascimentoValida(), 1);

        // Act
        cliente.AjustarPerfil(5);

        // Assert
        Assert.Equal(5, cliente.PerfilDeRiscoId);
    }

    [Fact]
    public void AjustarPerfil_DeveLancarExcecao_QuandoPerfilInvalido()
    {
        // Arrange
        var cliente = new Cliente(1, "João", "Silva", CriarEmailValido(), CriarCelularValido(), CriarCpfValido(), CriarDataNascimentoValida(), 1);

        // Act & Assert
        Assert.Throws<ArgumentException>(() => cliente.AjustarPerfil(0));
    }
}
