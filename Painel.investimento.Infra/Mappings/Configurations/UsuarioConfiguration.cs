using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Painel.Investimento.Domain.Models;

namespace Painel.Investimento.Infra.Data.Configurations
{
    public class UsuarioConfiguration : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            // Nome da tabela
            builder.ToTable("Usuarios");

            // Chave primária
            builder.HasKey(u => u.Id);

            // Propriedades
            builder.Property(u => u.Username)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(u => u.PasswordHash)
                   .IsRequired()
                   .HasMaxLength(255); // espaço para hash seguro (BCrypt, etc.)

            builder.Property(u => u.Role)
                   .IsRequired()
                   .HasMaxLength(50);

            // Índice único para Username
            builder.HasIndex(u => u.Username)
                   .IsUnique();
        }
    }
}
