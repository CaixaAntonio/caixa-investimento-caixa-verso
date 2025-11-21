using Microsoft.EntityFrameworkCore;
using Painel.investimento.Infra.Mappings.Configurations;
using Painel.Investimento.Domain.Models;
using Painel.Investimento.Infra.Configurations;
using Painel.Investimento.Infra.Data;
using Painel.Investimento.Infra.Data.Configurations;

namespace Painel.investimento.Infra.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Endereco> Enderecos { get; set; }
        public DbSet<PerfilDeRisco> PerfilDeRisco { get; set; }
        public DbSet<Investimentos> Investimentos { get; set; }
        public DbSet<ProdutoInvestimento> ProdutosInvestimento { get; set; }
        public DbSet<PerfilProduto> PerfilProdutos { get; set; }
        public DbSet<Transacao> Transacoes { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Simulacao> Simulacoes { get; set; }
       

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ClienteConfiguration());
            modelBuilder.ApplyConfiguration(new EnderecoConfiguration());
            modelBuilder.ApplyConfiguration(new PerfilDeRiscoConfiguration());
            modelBuilder.ApplyConfiguration(new InvestimentosConfiguration());
            modelBuilder.ApplyConfiguration(new ProdutoInvestimentoConfiguration());
            modelBuilder.ApplyConfiguration(new PerfilProdutoConfiguration());
            modelBuilder.ApplyConfiguration(new UsuarioConfiguration());
            modelBuilder.ApplyConfiguration(new TransacaoConfiguration());
        }

    }
}
