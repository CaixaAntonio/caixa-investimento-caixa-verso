using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using Xunit;
using Painel.Investimento.Aplication.UseCasesProdutos;
using Painel.Investimento.Domain.Models;
using Painel.Investimento.Domain.Repository.Abstract;

public class PerfilProdutoUseCaseTests
{
    private readonly Mock<IPerfilProdutoRepository> _repositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly PerfilProdutoUseCase _useCase;

    public PerfilProdutoUseCaseTests()
    {
        _repositoryMock = new Mock<IPerfilProdutoRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _useCase = new PerfilProdutoUseCase(_repositoryMock.Object, _unitOfWorkMock.Object);
    }

    [Fact]
    public async Task VincularAsync_DeveCriarEVincularPerfilProduto()
    {
        // Arrange
        int perfilId = 1;
        int produtoId = 10;

        // Act
        var resultado = await _useCase.VincularAsync(perfilId, produtoId);

        // Assert
        Assert.NotNull(resultado);
        Assert.Equal(perfilId, resultado.PerfilDeRiscoId);
        Assert.Equal(produtoId, resultado.ProdutoInvestimentoId);
        _repositoryMock.Verify(r => r.AddAsync(It.IsAny<PerfilProduto>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.CommitAsync(), Times.Once);
    }

    [Fact]
    public async Task ObterPorIdsAsync_DeveRetornarPerfilProduto()
    {
        // Arrange
        int perfilId = 1;
        int produtoId = 10;
        var perfilProduto = new PerfilProduto(perfilId, produtoId);

        _repositoryMock.Setup(r => r.GetByIdsAsync(perfilId, produtoId))
                       .ReturnsAsync(perfilProduto);

        // Act
        var resultado = await _useCase.ObterPorIdsAsync(perfilId, produtoId);

        // Assert
        Assert.NotNull(resultado);
        Assert.Equal(perfilId, resultado.PerfilDeRiscoId);
        Assert.Equal(produtoId, resultado.ProdutoInvestimentoId);
    }

    [Fact]
    public async Task ListarTodosAsync_DeveRetornarListaDePerfisProdutos()
    {
        // Arrange
        var lista = new List<PerfilProduto>
        {
            new PerfilProduto(1, 10),
            new PerfilProduto(2, 20)
        };

        _repositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(lista);

        // Act
        var resultado = await _useCase.ListarTodosAsync();

        // Assert
        Assert.NotNull(resultado);
        Assert.Equal(2, resultado.Count());
        Assert.Contains(resultado, p => p.PerfilDeRiscoId == 1 && p.ProdutoInvestimentoId == 10);
        Assert.Contains(resultado, p => p.PerfilDeRiscoId == 2 && p.ProdutoInvestimentoId == 20);
    }

    [Fact]
    public async Task RemoverAsync_DeveRemoverEVinculo_QuandoExiste()
    {
        // Arrange
        int perfilId = 1;
        int produtoId = 10;
        var perfilProduto = new PerfilProduto(perfilId, produtoId);

        _repositoryMock.Setup(r => r.GetByIdsAsync(perfilId, produtoId))
                       .ReturnsAsync(perfilProduto);

        // Act
        var resultado = await _useCase.RemoverAsync(perfilId, produtoId);

        // Assert
        Assert.True(resultado);
        _repositoryMock.Verify(r => r.Remove(perfilProduto), Times.Once);
        _unitOfWorkMock.Verify(u => u.CommitAsync(), Times.Once);
    }

    [Fact]
    public async Task RemoverAsync_DeveRetornarFalse_QuandoNaoExiste()
    {
        // Arrange
        int perfilId = 1;
        int produtoId = 10;

        _repositoryMock.Setup(r => r.GetByIdsAsync(perfilId, produtoId))
                       .ReturnsAsync((PerfilProduto?)null);

        // Act
        var resultado = await _useCase.RemoverAsync(perfilId, produtoId);

        // Assert
        Assert.False(resultado);
        _repositoryMock.Verify(r => r.Remove(It.IsAny<PerfilProduto>()), Times.Never);
        _unitOfWorkMock.Verify(u => u.CommitAsync(), Times.Never);
    }
}
