using Moq;
using Painel.Investimento.Aplication.UseCasesCadastros;
using Painel.Investimento.Domain.Models;
using Painel.Investimento.Domain.Repository.Abstract;
using Painel.Investimento.Domain.Valueobjects;

namespace Painel.Investimento.Testes.UseCaseTests
{
    public class ClienteUseCaseTests
    {
        private readonly Mock<IClienteRepository> _clienteRepoMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly ClienteUseCase _useCase;

        public ClienteUseCaseTests()
        {
            _clienteRepoMock = new Mock<IClienteRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();

            _useCase = new ClienteUseCase(_clienteRepoMock.Object, _unitOfWorkMock.Object);
        }

        [Fact]
        public async Task ExecuteAsync_DeveCriarCliente()
        {
            // Arrange
            var email = new Email("teste@teste.com");
            var celular = new Celular("31999999999");
            var cpf = new Cpf("12345678901");
            var nascimento = new DataDeNascimento(DateTime.Now.AddYears(-30));

            // Act
            var cliente = await _useCase.ExecuteAsync(1, "João", "Silva", email, celular, cpf, nascimento, 2);

            // Assert
            Assert.NotNull(cliente);
            Assert.Equal("João", cliente.Nome);
            _clienteRepoMock.Verify(r => r.AdicionarAsync(It.IsAny<Cliente>()), Times.Once);
            _unitOfWorkMock.Verify(u => u.CommitAsync(), Times.Once);
        }

        [Fact]
        public async Task ObterPorIdAsync_DeveRetornarCliente()
        {
            // Arrange
            var cliente = new Cliente(1, "Maria", "Oliveira", new Email("maria@teste.com"),
                new Celular("31988888888"), new Cpf("98765432100"), new DataDeNascimento(DateTime.Now.AddYears(-25)), 1);

            _clienteRepoMock.Setup(r => r.ObterPorIdAsync(1)).ReturnsAsync(cliente);

            // Act
            var resultado = await _useCase.ObterPorIdAsync(1);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal("Maria", resultado.Nome);
        }

        [Fact]
        public async Task ListarTodosAsync_DeveRetornarListaClientes()
        {
            // Arrange
            var clientes = new List<Cliente>
        {
            new Cliente(1, "Carlos", "Souza", new Email("carlos@teste.com"),
                new Celular("31977777777"), new Cpf("11122233344"), new DataDeNascimento(DateTime.Now.AddYears(-40)), 2)
        };

            _clienteRepoMock.Setup(r => r.ObterTodosAsync()).ReturnsAsync(clientes);

            // Act
            var resultado = await _useCase.ListarTodosAsync();

            // Assert
            Assert.Single(resultado);
        }

        [Fact]
        public async Task AtualizarPerfilAsync_DeveAlterarPerfil()
        {
            // Arrange
            var cliente = new Cliente(1, "Ana", "Costa", new Email("ana@teste.com"),
                new Celular("31966666666"), new Cpf("55566677788"), new DataDeNascimento(DateTime.Now.AddYears(-28)), 1);

            _clienteRepoMock.Setup(r => r.ObterPorIdAsync(1)).ReturnsAsync(cliente);

            // Act
            var atualizado = await _useCase.AtualizarPerfilAsync(1, 3);

            // Assert
            Assert.NotNull(atualizado);
            Assert.Equal(3, atualizado.PerfilDeRiscoId);
            _unitOfWorkMock.Verify(u => u.CommitAsync(), Times.Once);
        }

        [Fact]
        public async Task RemoverAsync_DeveRetornarTrue_QuandoClienteExiste()
        {
            // Arrange
            var cliente = new Cliente(1, "Pedro", "Alves", new Email("pedro@teste.com"),
                new Celular("31955555555"), new Cpf("99988877766"), new DataDeNascimento(DateTime.Now.AddYears(-35)), 2);

            _clienteRepoMock.Setup(r => r.ObterPorIdAsync(1)).ReturnsAsync(cliente);

            // Act
            var resultado = await _useCase.RemoverAsync(1);

            // Assert
            Assert.True(resultado);
            _clienteRepoMock.Verify(r => r.Remover(cliente), Times.Once);
            _unitOfWorkMock.Verify(u => u.CommitAsync(), Times.Once);
        }

        [Fact]
        public async Task RemoverAsync_DeveRetornarFalse_QuandoClienteNaoExiste()
        {
            // Arrange
            _clienteRepoMock.Setup(r => r.ObterPorIdAsync(1)).ReturnsAsync((Cliente?)null);

            // Act
            var resultado = await _useCase.RemoverAsync(1);

            // Assert
            Assert.False(resultado);
        }

        [Fact]
        public async Task AdicionarEnderecoAsync_DeveAdicionarEndereco()
        {
            // Arrange
            var cliente = new Cliente(1, "Lucas", "Ferreira", new Email("lucas@teste.com"),
                new Celular("31944444444"), new Cpf("12312312399"), new DataDeNascimento(DateTime.Now.AddYears(-20)), 1);

            _clienteRepoMock.Setup(r => r.ObterPorIdAsync(1)).ReturnsAsync(cliente);

            var endereco = new Endereco( "Rua A", "100", "Ap 1", "Centro", "BH", "MG", "30000000",1);

            // Act
            var resultado = await _useCase.AdicionarEnderecoAsync(1, endereco);

            // Assert
            Assert.NotNull(resultado);
            Assert.Contains(resultado.Enderecos, e => e.Logradouro == "Rua A");
            _unitOfWorkMock.Verify(u => u.CommitAsync(), Times.Once);
        }

        [Fact]
        public async Task RemoverEnderecoAsync_DeveRemoverEndereco()
        {
            // Arrange
            var cliente = new Cliente(1, "Julia", "Mendes", new Email("julia@teste.com"),
                new Celular("31933333333"), new Cpf("11122233399"), new DataDeNascimento(DateTime.Now.AddYears(-22)), 1);

            var endereco = new Endereco( "Rua B", "200", "", "Bairro", "BH", "MG", "30000001", 1);
            cliente.AdicionarEndereco(endereco);

            _clienteRepoMock.Setup(r => r.ObterPorIdAsync(1)).ReturnsAsync(cliente);

            // Act
            var resultado = await _useCase.RemoverEnderecoAsync(1, 1);

            // Assert
            Assert.NotNull(resultado);
            Assert.DoesNotContain(resultado.Enderecos, e => e.Id == 1);
            _unitOfWorkMock.Verify(u => u.CommitAsync(), Times.Once);
        }

        //[Fact]
        //public async Task AtualizarEnderecoAsync_DeveAtualizarEndereco()
        //{
        //    // Arrange
        //    var cliente = new Cliente(1, "Rafael", "Pereira", new Email("rafael@teste.com"),
        //        new Celular("31922222222"), new Cpf("22233344455"), new DataDeNascimento(DateTime.Now.AddYears(-29)), 1);

        //    var endereco = new Endereco( "Rua C", "300", "", "Bairro", "BH", "MG", "30000002",1);
        //    cliente.AdicionarEndereco(endereco);

        //    _clienteRepoMock.Setup(r => r.ObterPorIdAsync(1)).ReturnsAsync(cliente);

        //    // Act
        //    var resultado = await _useCase.AtualizarEnderecoAsync( 1,1, "Rua Atualizada", "400", "Casa", "Novo Bairro", "Nova Cidade", "SP", "40000000");

        //    // Assert
        //    Assert.NotNull(resultado);
        //    Assert.Contains(resultado.Enderecos, e => e.Logradouro == "Rua Atualizada");
        //    _unitOfWorkMock.Verify(u => u.CommitAsync(), Times.Once);
        //}
    }
}
