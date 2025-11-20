using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using Xunit;
using Painel.Investimento.Aplication.useCaseSimulacoes;
using Painel.Investimento.Domain.Dtos;
using Painel.Investimento.Domain.Repository.Abstract;

namespace Painel.Investimento.Testes.UseCaseTests
{
    public class ConsultarSimulacoesAgrupadasUseCaseTests
    {
        private readonly Mock<ISimulacaoRepository> _simulacaoRepoMock;
        private readonly ConsultarSimulacoesAgrupadasUseCase _useCase;

        public ConsultarSimulacoesAgrupadasUseCaseTests()
        {
            _simulacaoRepoMock = new Mock<ISimulacaoRepository>();
            _useCase = new ConsultarSimulacoesAgrupadasUseCase(_simulacaoRepoMock.Object);
        }

        [Fact]
        public async Task ExecuteAsync_DeveRetornarSimulacoesAgrupadas()
        {
            // Arrange
            var simulacoesAgrupadas = new List<SimulacaoPorDiaProdutoResponse>
            {
                new SimulacaoPorDiaProdutoResponse
                {
                    Produto = "CDB",
                    Data = DateTime.UtcNow.Date,
                    QuantidadeSimulacoes = 2,
                    MediaValorFinal = 1050
                },
                new SimulacaoPorDiaProdutoResponse
                {
                    Produto = "LCI",
                    Data = DateTime.UtcNow.Date,
                    QuantidadeSimulacoes = 1,
                    MediaValorFinal = 2300
                }
            };

            _simulacaoRepoMock
                .Setup(r => r.GetSimulacoesAgrupadasAsync())
                .ReturnsAsync(simulacoesAgrupadas);

            // Act
            var resultado = await _useCase.ExecuteAsync();

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(2, resultado.Count());
            Assert.Contains(resultado, s => s.Produto == "CDB" && s.QuantidadeSimulacoes == 2);
            Assert.Contains(resultado, s => s.Produto == "LCI" && s.MediaValorFinal == 2300);
        }

        [Fact]
        public async Task ExecuteAsync_DeveRetornarListaVazia_QuandoNaoExistemSimulacoes()
        {
            // Arrange
            _simulacaoRepoMock
                .Setup(r => r.GetSimulacoesAgrupadasAsync())
                .ReturnsAsync(new List<SimulacaoPorDiaProdutoResponse>());

            // Act
            var resultado = await _useCase.ExecuteAsync();

            // Assert
            Assert.NotNull(resultado);
            Assert.Empty(resultado);
        }
    }
}
