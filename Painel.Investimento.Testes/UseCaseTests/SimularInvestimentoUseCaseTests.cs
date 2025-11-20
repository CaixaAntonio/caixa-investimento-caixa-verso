using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using Xunit;
using Painel.Investimento.Aplication.useCaseSimulacoes;
using Painel.Investimento.Domain.Dtos;
using Painel.Investimento.Domain.Models;
using Painel.Investimento.Domain.Repository.Abstract;

public class SimularInvestimentoUseCaseTests
{
    private readonly Mock<IProdutoInvestimentoRepository> _produtoRepoMock;
    private readonly Mock<ISimulacaoRepository> _simulacaoRepoMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly SimularInvestimentoUseCase _useCase;

    public SimularInvestimentoUseCaseTests()
    {
        _produtoRepoMock = new Mock<IProdutoInvestimentoRepository>();
        _simulacaoRepoMock = new Mock<ISimulacaoRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();

        _useCase = new SimularInvestimentoUseCase(
            _produtoRepoMock.Object,
            _simulacaoRepoMock.Object,
            _unitOfWorkMock.Object
        );
    }

    [Fact]
    public async Task ExecuteAsync_DeveRetornarResponse_QuandoProdutoValido()
    {
        // Arrange
        var request = new SimulacaoInvestimentoRequest
        {
            ClienteId = 1,
            NomeDoProduto = "CDB",
            Valor = 1000,
            PrazoMeses = 12
        };

        var produto = new ProdutoInvestimento(
            nome: "CDB",
            tipoPerfil: "cdb",
            rentabilidadeAnual: 12,
            risco: 10,
            liquidez: "Diária",
            tributacao: "IR 15%",
            garantia: "FGC",
            descricao: "Produto de renda fixa"
        );

        _produtoRepoMock.Setup(r => r.GetByTipoAsync("CDB")).ReturnsAsync(produto);

        // Act
        var response = await _useCase.ExecuteAsync(request);

        // Assert
        Assert.NotNull(response);
        Assert.Equal("CDB", response.ProdutoValidado.Nome);
        Assert.Equal("cdb", response.ProdutoValidado.Tipo);
        Assert.Equal(12, response.ProdutoValidado.Rentabilidade);
        _simulacaoRepoMock.Verify(r => r.AddAsync(It.IsAny<Simulacao>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.CommitAsync(), Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_DeveLancarExcecao_QuandoProdutoNaoEncontrado()
    {
        // Arrange
        var request = new SimulacaoInvestimentoRequest
        {
            ClienteId = 1,
            NomeDoProduto = "Inexistente",
            Valor = 1000,
            PrazoMeses = 12
        };

        _produtoRepoMock.Setup(r => r.GetByTipoAsync("Inexistente")).ReturnsAsync((ProdutoInvestimento?)null);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _useCase.ExecuteAsync(request));
    }

    [Fact]
    public async Task CalcularRentabilidadeAsync_DeveRetornarRentabilidadeCorreta()
    {
        // Arrange
        var simulacao = new Simulacao(1, "CDB", 1000, 12, 1200);
        _simulacaoRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(simulacao);

        // Act
        var response = await _useCase.CalcularRentabilidadeAsync(simulacao.Id, minimoPercentual: 10);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(simulacao.Id, response.SimulacaoId);
        Assert.False(response.EhRentavel); // agora espera false
        Assert.Equal(simulacao.ValorFinal, response.ValorFinal);
    }




    [Fact]
    public async Task CalcularRentabilidadeAsync_DeveLancarExcecao_QuandoSimulacaoNaoEncontrada()
    {
        // Arrange
        _simulacaoRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Simulacao?)null);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _useCase.CalcularRentabilidadeAsync(99, 10));
    }

    [Fact]
    public async Task ListarTodasAsync_DeveRetornarSimulacoes()
    {
        // Arrange
        var simulacoes = new List<Simulacao>
        {
            new Simulacao(1, "CDB", 1000, 12, 1100),
            new Simulacao(1, "LCI", 2000, 24, 2300)
        };

        _simulacaoRepoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(simulacoes);

        // Act
        var resultado = await _useCase.ListarTodasAsync();

        // Assert
        Assert.NotNull(resultado);
        Assert.Equal(2, resultado.Count());
        Assert.Contains(resultado, s => s.Produto == "CDB");
        Assert.Contains(resultado, s => s.Produto == "LCI");
    }
}
