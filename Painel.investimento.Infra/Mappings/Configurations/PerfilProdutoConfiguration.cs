using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Painel.Investimento.Domain.Models;

namespace Painel.Investimento.Infra.Configurations
{
    public class PerfilProdutoConfiguration : IEntityTypeConfiguration<PerfilProduto>
    {
        public void Configure(EntityTypeBuilder<PerfilProduto> builder)
        {
            builder.ToTable("PerfilProduto");

            // ✅ Chave composta
            builder.HasKey(pp => new { pp.PerfilDeRiscoId, pp.ProdutoInvestimentoId });

            // ✅ Relacionamento com PerfilDeRisco
            builder.HasOne(pp => pp.PerfilDeRisco)
                   .WithMany(pr => pr.PerfilProdutos)
                   .HasForeignKey(pp => pp.PerfilDeRiscoId)
                   .OnDelete(DeleteBehavior.Cascade);

            // ✅ Relacionamento com ProdutoInvestimento
            builder.HasOne(pp => pp.ProdutoInvestimento)
                   .WithMany(pi => pi.PerfilProdutos)
                   .HasForeignKey(pp => pp.ProdutoInvestimentoId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
