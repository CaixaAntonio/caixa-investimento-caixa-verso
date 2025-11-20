using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Painel.Investimento.Domain.Models;

namespace Painel.Investimento.Infra.Configurations
{
    public class InvestimentosConfiguration : IEntityTypeConfiguration<Investimentos>
    {
        public void Configure(EntityTypeBuilder<Investimentos> builder)
        {
            builder.ToTable("Investimentos");

            // ✅ Chave primária
            builder.HasKey(i => i.Id);

            // ✅ Relacionamento com Cliente
            builder.HasOne(i => i.Cliente)
                   .WithMany(c => c.Investimentos)
                   .HasForeignKey(i => i.ClienteId)
                   .OnDelete(DeleteBehavior.Restrict);

            // ✅ Relacionamento com ProdutoInvestimento
            builder.HasOne(i => i.ProdutoInvestimento)
                   .WithMany(p => p.Investimentos)
                   .HasForeignKey(i => i.ProdutoInvestimentoId)
                   .OnDelete(DeleteBehavior.Restrict);

            // ✅ Propriedades
            builder.Property(i => i.ValorInvestido)
                   .HasColumnType("decimal(18,2)")
                   .IsRequired();

            builder.Property(i => i.DataInvestimento)
                   .IsRequired();

            builder.Property(i => i.PrazoMeses)
                   .IsRequired(false);

            builder.Property(i => i.Crise)
                   .IsRequired();

            builder.Property(i => i.ValorRetirado)
                   .HasColumnType("decimal(18,2)");

        }
    }
}
