using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Painel.Investimento.Domain.Models;

namespace Painel.investimento.Infra.Mappings.Configurations
{
    public class ClienteConfiguration : IEntityTypeConfiguration<Cliente>
    {
        public void Configure(EntityTypeBuilder<Cliente> builder)
        {
            builder.ToTable("Cliente");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Nome)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(c => c.Sobrenome)
                   .IsRequired()
                   .HasMaxLength(100);

            // Email como Owned Type
            builder.OwnsOne(c => c.Email, email =>
            {
                email.Property(e => e.Value)
                     .HasColumnName("Email")
                     .IsRequired()
                     .HasMaxLength(150);
            });

            // Celular como Owned Type
            builder.OwnsOne(c => c.Celular, celular =>
            {
                celular.Property(e => e.Numero)
                       .HasColumnName("Celular")
                       .IsRequired()
                       .HasMaxLength(11);
            });

            // CPF como Owned Type
            builder.OwnsOne(c => c.Cpf, cpf =>
            {
                cpf.Property(e => e.Numero)
                   .HasColumnName("Cpf")
                   .IsRequired()
                   .HasMaxLength(11);
            });

            // Data de Nascimento
            builder.OwnsOne(c => c.DataDeNascimento, dn =>
            {
                dn.Property(d => d.Valor)
                  .HasColumnName("DataDeNascimento")
                  .IsRequired();
            });

            // Idade (armazenar valor calculado)
            builder.OwnsOne(c => c.Idade, idade =>
            {
                idade.Property(i => i.Valor)
                     .HasColumnName("Idade")
                     .IsRequired();
            });
                        
            // Relacionamento 1:N com Endereços
            builder.HasMany(c => c.Enderecos)
                   .WithOne(e => e.Cliente)
                   .HasForeignKey(e => e.ClienteId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.Property(c => c.PerfilDeRiscoId)
                  .IsRequired()
                  .HasMaxLength(150);

            // Relacionamento com Transacoes
            builder.HasMany<Transacao>()
                   .WithOne()
                   .HasForeignKey(t => t.ClienteId)
                   .OnDelete(DeleteBehavior.Cascade);




        }
    }
}