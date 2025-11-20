using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Painel.Investimento.Domain.Models;

namespace Painel.Investimento.Infra.Data.Configurations
{
    public class TransacaoConfiguration : IEntityTypeConfiguration<Transacao>
    {
        public void Configure(EntityTypeBuilder<Transacao> builder)
        {
            // Nome da tabela
            builder.ToTable("Transacoes");

            // Chave primária
            builder.HasKey(t => t.Id);

            // Propriedades
            builder.Property(t => t.Valor)
                   .IsRequired()
                   .HasColumnType("decimal(18,2)");

            builder.Property(t => t.Data)
                   .IsRequired();

            builder.Property(t => t.Tipo)
                   .IsRequired()
                   .HasMaxLength(50);

            // Relacionamento com Cliente
            builder.HasOne<Cliente>()
                   .WithMany() // se Cliente tiver ICollection<Transacao>, pode usar .WithMany(c => c.Transacoes)
                   .HasForeignKey(t => t.ClienteId)
                   .OnDelete(DeleteBehavior.Cascade);

            // Índices
            builder.HasIndex(t => t.ClienteId);
            builder.HasIndex(t => t.Data);
        }
    }
}
