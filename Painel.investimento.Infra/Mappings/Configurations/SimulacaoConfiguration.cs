using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Painel.Investimento.Domain.Models;

namespace Painel.Investimento.Infra.Configurations
{
    public class SimulacaoConfiguration : IEntityTypeConfiguration<Simulacao>
    {
        public void Configure(EntityTypeBuilder<Simulacao> builder)
        {
            builder.ToTable("Simulacoes");

            builder.HasKey(s => s.Id);

            builder.Property(s => s.ClienteId)
                   .IsRequired();

            builder.Property(s => s.NomeProduto)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(s => s.ValorInicial)
                   .HasPrecision(18, 2)
                   .IsRequired();

            builder.Property(s => s.ValorFinal)
                   .HasPrecision(18, 2)
                   .IsRequired();

            builder.Property(s => s.RentabilidadeEfetiva)
                   .HasPrecision(18, 2)
                   .IsRequired();

            builder.Property(s => s.PrazoMeses)
                   .IsRequired();

            builder.Property(s => s.DataSimulacao)
                   .HasColumnType("datetime2")
                   .IsRequired();

            
        }
    }
}
