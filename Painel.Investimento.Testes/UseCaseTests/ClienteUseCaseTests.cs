using Moq;
using Xunit;
using Microsoft.Extensions.Logging;
using Painel.Investimento.Aplication.UseCasesCadastros;
using Painel.Investimento.Domain.Models;
using Painel.Investimento.Domain.Repository.Abstract;
using Painel.Investimento.Domain.Valueobjects;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Painel.Investimento.Testes.UseCaseTests
{
    public class ClienteUseCaseTests
    {
        private readonly Mock<IClienteRepository> _repositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<ILogger<ClienteUseCase>> _loggerMock;
        private readonly ClienteUseCase _useCase;

        public ClienteUseCaseTests()
        {
            _repositoryMock = new Mock<IClienteRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _loggerMock = new Mock<ILogger<ClienteUseCase>>();

            _useCase = new ClienteUseCase(
                _repositoryMock.Object,
                _unitOfWorkMock.Object,
                _loggerMock.Object
            );
        }

        //[Fact]
        //public async Task ExecuteAsync_DeveCriarCliente()
        //{
        //    // Arrange
        //    var email = new Email("teste@teste.com");
        //    var celular = new Celular("31999999999");
        //    var cpf = new Cpf("12345678901");
        //    var nascimento = new DataDeNascimento(DateTime.Now.AddYears(-30));

        //    _repositoryMock.Setup(r => r.AdicionarAsync(It.IsAny<Cliente>()))
        //        .ReturnsAsync(1); // ✅ corrigido

        //    _unitOfWorkMock.Setup(u => u.CommitAsync())
        //        .Returns(Task.CompletedTask);

        //    // Act
        //    var cliente = await _useCase.ExecuteAsync(1, "Antonio", "Silva", email, celular, cpf, nascimento, 2);

        //    // Assert
        //    Assert.NotNull(cliente);
        //    Assert.Equal("Antonio", cliente.Nome);
        //    _repositoryMock.Verify(r => r.AdicionarAsync(It.IsAny<Cliente>()), Times.Once);
        //    _unitOfWorkMock.Verify(u => u.CommitAsync(), Times.Once);
        //}


        [Fact]
        public async Task ObterPorIdAsync_DeveRetornarCliente()
        {
            // Arrange
            var cliente = new Cliente(1, "Antonio", "Silva", new Email("teste@teste.com"),
                new Celular("31999999999"), new Cpf("12345678901"), new DataDeNascimento(DateTime.Now.AddYears(-30)), 2);

            _repositoryMock.Setup(r => r.ObterPorIdAsync(1))
                .ReturnsAsync(cliente);

            // Act
            var resultado = await _useCase.ObterPorIdAsync(1);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal("Antonio", resultado.Nome);
        }

        //[Fact]
        //public async Task AtualizarPerfilAsync_DeveAtualizarPerfil()
        //{
        //    // Arrange
        //    var cliente = new Cliente(1, "Antonio", "Silva", new Email("teste@teste.com"),
        //        new Celular("31999999999"), new Cpf("12345678901"), new DataDeNascimento(DateTime.Now.AddYears(-30)), 2);

        //    _repositoryMock.Setup(r => r.ObterPorIdAsync(1))
        //        .ReturnsAsync(cliente);

        //    _unitOfWorkMock.Setup(u => u.CommitAsync())
        //        .Returns(Task.CompletedTask);

        //    // Act
        //    var resultado = await _useCase.AtualizarPerfilAsync(1, 3);

        //    // Assert
        //    Assert.NotNull(resultado);
        //    Assert.Equal(3, resultado.PerfilDeRiscoId);
        //    _unitOfWorkMock.Verify(u => u.CommitAsync(), Times.Once);
        //}

        //[Fact]
        //public async Task RemoverAsync_DeveRemoverCliente()
        //{
        //    // Arrange
        //    var cliente = new Cliente(1, "Antonio", "Silva", new Email("teste@teste.com"),
        //        new Celular("31999999999"), new Cpf("12345678901"), new DataDeNascimento(DateTime.Now.AddYears(-30)), 2);

        //    _repositoryMock.Setup(r => r.ObterPorIdAsync(1))
        //        .ReturnsAsync(cliente);

        //    _unitOfWorkMock.Setup(u => u.CommitAsync())
        //        .Returns(Task.CompletedTask);

        //    // Act
        //    var resultado = await _useCase.RemoverAsync(1);

        //    // Assert
        //    Assert.True(resultado);
        //    _repositoryMock.Verify(r => r.Remover(cliente), Times.Once);
        //    _unitOfWorkMock.Verify(u => u.CommitAsync(), Times.Once);
        //}
    }
}
