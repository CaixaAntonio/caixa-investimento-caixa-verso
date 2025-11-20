using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Painel.Investimento.Domain.Models;

namespace Painel.Investimento.Infra.Configurations
{
    public class ProdutoInvestimentoConfiguration : IEntityTypeConfiguration<ProdutoInvestimento>
    {
        public void Configure(EntityTypeBuilder<ProdutoInvestimento> builder)
        {
            builder.ToTable("ProdutoInvestimento");

            // ✅ Chave primária
            builder.HasKey(p => p.Id);

            // ✅ Propriedades
            builder.Property(p => p.Nome)
                   .HasMaxLength(150)
                   .IsRequired();

            builder.Property(p => p.Tipo)
                   .HasMaxLength(50)
                   .IsRequired();

            builder.Property(p => p.RentabilidadeAnual)
                   .HasColumnType("decimal(18,2)")
                   .IsRequired();

            builder.Property(p => p.Risco)
                   .IsRequired();

            builder.Property(p => p.Liquidez)
                   .HasMaxLength(50)
                   .IsRequired();

            builder.Property(p => p.Tributacao)
                   .HasMaxLength(100)
                   .IsRequired();

            builder.Property(p => p.Garantia)
                   .HasMaxLength(100)
                   .IsRequired();

            builder.Property(p => p.Descricao)
                   .HasMaxLength(500)
                   .IsRequired();

            // ✅ Relacionamento com PerfilProduto (já existia)
            builder.HasMany(p => p.PerfilProdutos)
                   .WithOne(pp => pp.ProdutoInvestimento)
                   .HasForeignKey(pp => pp.ProdutoInvestimentoId)
                   .OnDelete(DeleteBehavior.Cascade);

            // ✅ Novo: relacionamento com Investimentos
            builder.HasMany(p => p.Investimentos)
                   .WithOne(i => i.ProdutoInvestimento)
                   .HasForeignKey(i => i.ProdutoInvestimentoId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
