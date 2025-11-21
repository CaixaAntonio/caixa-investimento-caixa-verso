using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using Xunit;
using Microsoft.Extensions.Logging;
using Painel.Investimento.Aplication.useCaseSimulacoes;
using Painel.Investimento.Domain.Dtos;
using Painel.Investimento.Domain.Repository.Abstract;

namespace Painel.Investimento.Testes.UseCaseTests
{
    public class ConsultarSimulacoesAgrupadasUseCaseTests
    {
        private readonly Mock<ISimulacaoRepository> _simulacaoRepoMock;
        private readonly Mock<ILogger<ConsultarSimulacoesAgrupadasUseCase>> _loggerMock;
        private readonly ConsultarSimulacoesAgrupadasUseCase _useCase;

        public ConsultarSimulacoesAgrupadasUseCaseTests()
        {
            _simulacaoRepoMock = new Mock<ISimulacaoRepository>();
            _loggerMock = new Mock<ILogger<ConsultarSimulacoesAgrupadasUseCase>>();

            _useCase = new ConsultarSimulacoesAgrupadasUseCase(
                _simulacaoRepoMock.Object,
                _loggerMock.Object
            );
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

            // Verifica se o logger foi chamado
            _loggerMock.Verify(
                        l => l.Log(
                            LogLevel.Information,
                            It.IsAny<EventId>(),
                            It.IsAny<It.IsAnyType>(),
                            It.IsAny<Exception>(),
                            It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                        Times.AtLeastOnce);

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

            // Verifica se o logger registrou o aviso
           
               _loggerMock.Verify(
                l => l.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.IsAny<It.IsAnyType>(),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.AtLeastOnce);
                    

        }

    }
}
