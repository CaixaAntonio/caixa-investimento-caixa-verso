using Moq;
using Painel.Investimento.Aplication.useCaseSimulacoes;
using Painel.Investimento.Domain.Dtos;
using Painel.Investimento.Domain.Repository.Abstract;

namespace Painel.Investimento.Testes.UseCaseTests
{
    public class ConsultarHistoricoSimulacoesUseCaseTests
    {
        private readonly Mock<ISimulacaoRepository> _simulacaoRepoMock;
        private readonly ConsultarHistoricoSimulacoesUseCase _useCase;

        public ConsultarHistoricoSimulacoesUseCaseTests()
        {
            _simulacaoRepoMock = new Mock<ISimulacaoRepository>();
            _useCase = new ConsultarHistoricoSimulacoesUseCase(_simulacaoRepoMock.Object);
        }

        
        [Fact]
        public async Task ExecuteAsync_DeveRetornarSimulacoesDoCliente()
        {
            // Arrange
            int clienteId = 1;
            var simulacoes = new List<SimulacaoInvestimentoResponse>
{
            new SimulacaoInvestimentoResponse
            {
                ProdutoValidado = new ProdutoValidadoDto
                {
                    Id = 1,
                    Nome = "CDB",
                    Tipo = "Renda Fixa",
                    Rentabilidade = 0.10m,
                    Risco = "Baixo"
                },
                ResultadoSimulacao = new ResultadoSimulacaoDto
                {
                    ValorFinal = 1100,
                    RentabilidadeEfetiva = ((1100 - 1000) / 1000m) * 100,
                    PrazoMeses = 12
                },
                DataSimulacao = DateTime.UtcNow
            },
            new SimulacaoInvestimentoResponse
            {
                ProdutoValidado = new ProdutoValidadoDto
                {
                    Id = 2,
                    Nome = "LCI",
                    Tipo = "Renda Fixa",
                    Rentabilidade = 0.15m,
                    Risco = "Baixo"
                },
                ResultadoSimulacao = new ResultadoSimulacaoDto
                {
                    ValorFinal = 2300,
                    RentabilidadeEfetiva = ((2300 - 2000) / 2000m) * 100,
                    PrazoMeses = 24
                },
                DataSimulacao = DateTime.UtcNow
            }
            };


            _simulacaoRepoMock
                .Setup(r => r.GetByClienteIdAsync(It.IsAny<int>()))
                .ReturnsAsync(simulacoes);

            // Act
            var resultado = await _useCase.ExecuteAsync(clienteId);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(clienteId, resultado.ClienteId);
            Assert.Equal(2, resultado.Simulacoes.Count());

            // Verifica se contém produtos esperados
            Assert.Contains(resultado.Simulacoes, s => s.ProdutoValidado.Nome == "CDB");
            Assert.Contains(resultado.Simulacoes, s => s.ProdutoValidado.Nome == "LCI");

        }

        [Fact]
        public async Task ExecuteAsync_DeveRetornarListaVazia_QuandoNaoExistemSimulacoes()
        {
            // Arrange
            int clienteId = 2;
            _simulacaoRepoMock.Setup(r => r.GetByClienteIdAsync(clienteId))
                .ReturnsAsync(new List<SimulacaoInvestimentoResponse>());

            // Act
            var resultado = await _useCase.ExecuteAsync(clienteId);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(clienteId, resultado.ClienteId);
            Assert.Empty(resultado.Simulacoes);
        }
    }

}
