using Moq;
using Painel.Investimento.Aplication.UseCaseInvestimentos;
using Painel.Investimento.Domain.Models;
using Painel.Investimento.Domain.Repository.Abstract;
using Painel.Investimento.Domain.Services;

namespace Painel.Investimento.Testes.UseCaseTests
{
    public class CalcularPerfilDeRiscoUseCaseTests
    {
        private readonly Mock<IInvestimentosRepository> _investimentoRepoMock;
        private readonly Mock<IPerfilDeRiscoRepository> _perfilRepoMock;
        private readonly Mock<IRiskProfileService> _riskServiceMock;
        private readonly CalcularPerfilDeRiscoUseCase _useCase;

        public CalcularPerfilDeRiscoUseCaseTests()
        {
            _investimentoRepoMock = new Mock<IInvestimentosRepository>();
            _perfilRepoMock = new Mock<IPerfilDeRiscoRepository>();
            _riskServiceMock = new Mock<IRiskProfileService>();

            _useCase = new CalcularPerfilDeRiscoUseCase(
                _investimentoRepoMock.Object,
                _perfilRepoMock.Object,
                _riskServiceMock.Object
            );
        }

        [Fact]
        public void CalcularPontuacao_DeveRetornarPontuacaoCorreta()
        {
            // Arrange
            var investimentos = new List<Investimentos>
        {
            new Investimentos(1, 1, 600, DateTime.Now.AddMonths(-6), 12), // Valor >= 500 => +20, Prazo >= 12 => +10
            new Investimentos(1, 2, 200, DateTime.Now.AddMonths(-24), 24), // Valor >= 100 => +10, Prazo >= 24 => +15
            new Investimentos(1, 3, 50, DateTime.Now.AddMonths(-3), 6)    // Valor > 0 => +5, Prazo < 12 => +5
        };

            // Act
            var pontuacao = _useCase.CalcularPontuacao(investimentos);

            // Assert
            Assert.Equal(65, pontuacao); // 20+10 + 10+15 + 5+5 = 65
        }

        [Theory]
        [InlineData(20, "Conservador, Perfil de baixo risco")]
        [InlineData(50, "Moderado, Perfil de risco moderado")]
        [InlineData(80, "Agressivo, Perfil de risco agressivo e alto investimento")]
        public void ClassificarPerfil_DeveRetornarDescricaoCorreta(int pontuacao, string esperado)
        {
            // Act
            var resultado = _useCase.ClassificarPerfil(pontuacao);

            // Assert
            Assert.Equal(esperado, resultado);
        }

        [Fact]
        public async Task ExecuteAsync_DeveRetornarPerfilCorreto()
        {
            // Arrange
            var clienteId = 1;
            var investimentos = new List<Investimentos>
        {
            new Investimentos(clienteId, 1, 600, DateTime.Now, 12)
        };

            var perfis = new List<PerfilDeRisco>
        {
            new PerfilDeRisco(1, "Conservador", 0, 35, "Perfil de baixo risco"),
            new PerfilDeRisco(2, "Moderado", 36, 65, "Perfil de risco moderado"),
            new PerfilDeRisco(3, "Agressivo", 66, 100, "Perfil de risco agressivo")
        };

            _investimentoRepoMock.Setup(r => r.ObterPorClienteAsync(clienteId))
                .ReturnsAsync(investimentos);

            _perfilRepoMock.Setup(r => r.GetAllAsync())
                .ReturnsAsync(perfis);

            _riskServiceMock.Setup(s => s.DeterminarPerfil(It.IsAny<int>(), perfis))
                .Returns(perfis[1]); // Simula retorno do perfil Moderado

            // Act
            var resultado = await _useCase.ExecuteAsync(clienteId);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal("Moderado", resultado.Nome);
        }
    }
}
