using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Painel.Investimento.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Painel.investimento.Infra.Mappings.Configurations
{

    public class PerfilDeRiscoConfiguration : IEntityTypeConfiguration<PerfilDeRisco>
    {
        public void Configure(EntityTypeBuilder<PerfilDeRisco> builder)
        {
            builder.ToTable("PerfilDeRisco");
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Nome).HasMaxLength(50).IsRequired();
            builder.Property(p => p.PontuacaoMinima).HasMaxLength(10).IsRequired();
            builder.Property(p => p.PontuacaoMaxima).HasMaxLength(10).IsRequired();
            builder.Property(p => p.Descricao).HasMaxLength(200);
        }
    }

}
