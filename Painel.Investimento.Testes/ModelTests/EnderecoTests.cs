using System;
using Xunit;
using Painel.Investimento.Domain.Models;

public class EnderecoTests
{
    [Fact]
    public void Construtor_DeveCriarEnderecoValido()
    {
        // Act
        var endereco = new Endereco(
            logradouro: "Rua A",
            numero: "100",
            complemento: "Ap 1",
            bairro: "Centro",
            cidade: "Belo Horizonte",
            estado: "MG",
            cep: "30000-000",
            clienteId: 1
        );

        // Assert
        Assert.Equal("Rua A", endereco.Logradouro);
        Assert.Equal("100", endereco.Numero);
        Assert.Equal("Ap 1", endereco.Complemento);
        Assert.Equal("Centro", endereco.Bairro);
        Assert.Equal("Belo Horizonte", endereco.Cidade);
        Assert.Equal("MG", endereco.Estado);
        Assert.Equal("30000-000", endereco.Cep);
        Assert.Equal(1, endereco.ClienteId);
    }

    [Fact]
    public void Atualizar_DeveAlterarCamposDoEndereco()
    {
        // Arrange
        var endereco = new Endereco(
            logradouro: "Rua A",
            numero: "100",
            complemento: "Ap 1",
            bairro: "Centro",
            cidade: "Belo Horizonte",
            estado: "MG",
            cep: "30000-000",
            clienteId: 1
        );

        // Act
        endereco.Atualizar(
            logradouro: "Rua B",
            numero: "200",
            complemento: "Casa",
            bairro: "Bairro Novo",
            cidade: "São Paulo",
            estado: "SP",
            cep: "40000-000"
        );

        // Assert
        Assert.Equal("Rua B", endereco.Logradouro);
        Assert.Equal("200", endereco.Numero);
        Assert.Equal("Casa", endereco.Complemento);
        Assert.Equal("Bairro Novo", endereco.Bairro);
        Assert.Equal("São Paulo", endereco.Cidade);
        Assert.Equal("SP", endereco.Estado);
        Assert.Equal("40000-000", endereco.Cep);
    }
}
