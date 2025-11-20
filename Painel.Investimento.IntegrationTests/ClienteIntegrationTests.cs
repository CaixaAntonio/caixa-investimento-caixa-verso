using Microsoft.EntityFrameworkCore;
using Painel.investimento.Infra.Data;
using Painel.Investimento.Domain.Models;
using Painel.Investimento.Domain.Valueobjects;
using Painel.Investimento.IntegrationTests.Context;

namespace Painel.Investimento.IntegrationTests
{
    public class ClienteIntegrationTests : IClassFixture<IntegrationTestFixture>
    {
        private readonly AppDbContext _context;

        public ClienteIntegrationTests(IntegrationTestFixture fixture)
        {
            _context = fixture.Context;
        }
       
        [Fact]
        public async Task DeveCriarClienteComEnderecoInvestimentoEPerfilDeRisco()
        {
            var context = _context; // usar o fixture

            // Arrange - Cliente
            var cliente = new Cliente(
                id: 1,
                nome: "João",
                sobrenome: "Silva",
                email: new Email("joao@teste.com"),
                celular: new Celular("31999999999"),
                cpf: new Cpf("12345678901"),
                dataDeNascimento: new DataDeNascimento(new DateTime(1990, 1, 1)),
                perfilDeRiscoId: 1
            );

            var endereco = new Endereco("Rua A", "100", "Ap 1", "Centro", "BH", "MG", "30000-000", cliente.Id);
            cliente.AdicionarEndereco(endereco);

            var produto = new ProdutoInvestimento("CDB", "Renda Fixa", 12, 10, "Diária", "IR", "FGC", "Desc");
            var perfil = new PerfilDeRisco(1, "Conservador", 0, 20, "Baixo risco");

            // Persistir produto e perfil primeiro para gerar Ids
            context.ProdutosInvestimento.Add(produto);
            context.PerfilDeRisco.Add(perfil);
            await context.SaveChangesAsync();

            var perfilProduto = new PerfilProduto(perfil.Id, produto.Id!.Value);
            var investimento = new Investimentos(cliente.Id, produto.Id!.Value, 1000, DateTime.UtcNow, 12);
            cliente.AdicionarInvestimento(investimento);

            // Persistir cliente e vínculos
            context.Clientes.Add(cliente);
            context.PerfilProdutos.Add(perfilProduto);
            await context.SaveChangesAsync();

            // Assert
            var clienteDb = await context.Clientes
                .Include(c => c.Enderecos)
                .Include(c => c.Investimentos)
                .FirstOrDefaultAsync(c => c.Id == cliente.Id);

            Assert.NotNull(clienteDb);
            Assert.Single(clienteDb.Enderecos);
            Assert.Single(clienteDb.Investimentos);

            var produtoDb = await context.ProdutosInvestimento.FirstOrDefaultAsync(p => p.Id == produto.Id);
            Assert.NotNull(produtoDb);

            var perfilDb = await context.PerfilDeRisco
                .Include(p => p.PerfilProdutos)
                .FirstOrDefaultAsync(p => p.Id == perfil.Id);

            Assert.NotNull(perfilDb);
            Assert.Single(perfilDb.PerfilProdutos);
        }

    }

}
