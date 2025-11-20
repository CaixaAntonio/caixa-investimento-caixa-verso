using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using Xunit;
using Painel.Investimento.Aplication.UseCasesProdutos;
using Painel.Investimento.Domain.Models;
using Painel.Investimento.Domain.Repository.Abstract;

public class ProdutoInvestimentoUseCaseTests
{
    private readonly Mock<IProdutoInvestimentoRepository> _repositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly ProdutoInvestimentoUseCase _useCase;

    public ProdutoInvestimentoUseCaseTests()
    {
        _repositoryMock = new Mock<IProdutoInvestimentoRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _useCase = new ProdutoInvestimentoUseCase(_repositoryMock.Object, _unitOfWorkMock.Object);
    }

    [Fact]
    public async Task ExecuteAsync_DeveCriarProdutoEChamarRepositorio()
    {
        // Act
        var produto = await _useCase.ExecuteAsync(
            nome: "CDB",
            tipo: "Renda Fixa",
            rentabilidadeAnual: 12,
            risco: 10,
            liquidez: "Diária",
            tributacao: "IR 15%",
            garantia: "FGC",
            descricao: "Produto de renda fixa"
        );

        // Assert
        Assert.NotNull(produto);
        Assert.Equal("CDB", produto.Nome);
        Assert.Equal("Renda Fixa", produto.Tipo);
        _repositoryMock.Verify(r => r.AddAsync(It.IsAny<ProdutoInvestimento>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.CommitAsync(), Times.Once);
    }

    [Fact]
    public async Task ObterPorIdAsync_DeveRetornarProduto()
    {
        // Arrange
        var produto = new ProdutoInvestimento("CDB", "Renda Fixa", 12, 10, "Diária", "IR 15%", "FGC", "Desc");
        _repositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(produto);

        // Act
        var resultado = await _useCase.ObterPorIdAsync(1);

        // Assert
        Assert.NotNull(resultado);
        Assert.Equal("CDB", resultado.Nome);
    }

    [Fact]
    public async Task ListarTodosAsync_DeveRetornarListaDeProdutos()
    {
        // Arrange
        var lista = new List<ProdutoInvestimento>
        {
            new ProdutoInvestimento("CDB", "Renda Fixa", 12, 10, "Diária", "IR 15%", "FGC", "Desc"),
            new ProdutoInvestimento("LCI", "Renda Fixa", 15, 20, "Mensal", "Isento", "FGC", "Desc")
        };

        _repositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(lista);

        // Act
        var resultado = await _useCase.ListarTodosAsync();

        // Assert
        Assert.NotNull(resultado);
        Assert.Equal(2, resultado.Count());
        Assert.Contains(resultado, p => p.Nome == "CDB");
        Assert.Contains(resultado, p => p.Nome == "LCI");
    }

    [Fact]
    public async Task AtualizarAsync_DeveAtualizarProdutoExistente()
    {
        // Arrange
        var produto = new ProdutoInvestimento("CDB", "Renda Fixa", 12, 10, "Diária", "IR 15%", "FGC", "Desc");
        _repositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(produto);

        // Act
        var resultado = await _useCase.AtualizarAsync(
            id: 1,
            nome: "LCI",
            tipo: "Renda Fixa",
            rentabilidadeAnual: 15,
            risco: 20,
            liquidez: "Mensal",
            tributacao: "Isento",
            garantia: "FGC",
            descricao: "Produto atualizado"
        );

        // Assert
        Assert.NotNull(resultado);
        Assert.Equal("LCI", resultado.Nome);
        Assert.Equal(15, resultado.RentabilidadeAnual);
        _unitOfWorkMock.Verify(u => u.CommitAsync(), Times.Once);
    }

    [Fact]
    public async Task AtualizarAsync_DeveRetornarNull_QuandoProdutoNaoExiste()
    {
        // Arrange
        _repositoryMock.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((ProdutoInvestimento?)null);

        // Act
        var resultado = await _useCase.AtualizarAsync(
            id: 99,
            nome: "LCI",
            tipo: "Renda Fixa",
            rentabilidadeAnual: 15,
            risco: 20,
            liquidez: "Mensal",
            tributacao: "Isento",
            garantia: "FGC",
            descricao: "Produto atualizado"
        );

        // Assert
        Assert.Null(resultado);
        _unitOfWorkMock.Verify(u => u.CommitAsync(), Times.Never);
    }

    [Fact]
    public async Task RemoverAsync_DeveRemoverProdutoExistente()
    {
        // Arrange
        var produto = new ProdutoInvestimento("CDB", "Renda Fixa", 12, 10, "Diária", "IR 15%", "FGC", "Desc");
        _repositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(produto);

        // Act
        var resultado = await _useCase.RemoverAsync(1);

        // Assert
        Assert.True(resultado);
        _repositoryMock.Verify(r => r.Remove(produto), Times.Once);
        _unitOfWorkMock.Verify(u => u.CommitAsync(), Times.Once);
    }

    [Fact]
    public async Task RemoverAsync_DeveRetornarFalse_QuandoProdutoNaoExiste()
    {
        // Arrange
        _repositoryMock.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((ProdutoInvestimento?)null);

        // Act
        var resultado = await _useCase.RemoverAsync(99);

        // Assert
        Assert.False(resultado);
        _repositoryMock.Verify(r => r.Remove(It.IsAny<ProdutoInvestimento>()), Times.Never);
        _unitOfWorkMock.Verify(u => u.CommitAsync(), Times.Never);
    }
}
