using Moq;
using Painel.Investimento.Aplication.UseCaseInvestimentos;
using Painel.Investimento.Domain.Models;
using Painel.Investimento.Domain.Repository.Abstract;

namespace Painel.Investimento.Testes.UseCaseTests
{
    public class InvestimentosUseCaseTests
    {
        private readonly Mock<IInvestimentosRepository> _investimentosRepoMock;
        private readonly Mock<IClienteRepository> _clienteRepoMock;
        private readonly Mock<IProdutoInvestimentoRepository> _produtoRepoMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly InvestimentosUseCase _useCase;

        public InvestimentosUseCaseTests()
        {
            _investimentosRepoMock = new Mock<IInvestimentosRepository>();
            _clienteRepoMock = new Mock<IClienteRepository>();
            _produtoRepoMock = new Mock<IProdutoInvestimentoRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();

            _useCase = new InvestimentosUseCase(
                _investimentosRepoMock.Object,
                _clienteRepoMock.Object,
                _produtoRepoMock.Object,
                _investimentosRepoMock.Object, // usado também como _investimento
                _unitOfWorkMock.Object
            );
        }

        

        [Fact]
        public async Task RegistrarAsync_DeveLancarExcecao_QuandoClienteNaoExiste()
        {
            // Arrange
            _clienteRepoMock.Setup(r => r.ObterPorIdAsync(1)).ReturnsAsync((Cliente?)null);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() =>
                _useCase.RegistrarAsync(1, 1, 1000, DateTime.Now, 12));
        }

        [Fact]
        public void CalcularRentabilidade_DeveRetornarCorreto()
        {
            // Arrange
            decimal valorInvestido = 1000;
            decimal valorRetirado = 1200;

            // Act
            var resultado = _useCase.CalcularRentabilidade(valorInvestido, valorRetirado);

            // Assert
            Assert.Equal(0.2m, resultado); // 20% de rentabilidade
        }

        [Fact]
        public async Task AtualizarAsync_DeveAlterarValores()
        {
            // Arrange
            var investimento = new Investimentos(1, 1, 1000, DateTime.Now, 12);
            _investimentosRepoMock.Setup(r => r.ObterPorIdAsync(1)).ReturnsAsync(investimento);

            // Act
            var atualizado = await _useCase.AtualizarAsync(1, 2000, 24);

            // Assert
            Assert.NotNull(atualizado);
            Assert.Equal(2000, atualizado.ValorInvestido);
            Assert.Equal(24, atualizado.PrazoMeses);
            _unitOfWorkMock.Verify(u => u.CommitAsync(), Times.Once);
        }

        [Fact]
        public async Task RemoverAsync_DeveRetornarTrue_QuandoInvestimentoExiste()
        {
            // Arrange
            var investimento = new Investimentos(1, 1, 1000, DateTime.Now, 12);
            _investimentosRepoMock.Setup(r => r.ObterPorIdAsync(1)).ReturnsAsync(investimento);

            // Act
            var resultado = await _useCase.RemoverAsync(1);

            // Assert
            Assert.True(resultado);
            _investimentosRepoMock.Verify(r => r.RemoverAsync(1), Times.Once);
            _unitOfWorkMock.Verify(u => u.CommitAsync(), Times.Once);
        }

        [Fact]
        public async Task RemoverAsync_DeveRetornarFalse_QuandoInvestimentoNaoExiste()
        {
            // Arrange
            _investimentosRepoMock.Setup(r => r.ObterPorIdAsync(1)).ReturnsAsync((Investimentos?)null);

            // Act
            var resultado = await _useCase.RemoverAsync(1);

            // Assert
            Assert.False(resultado);
        }
    }
}
