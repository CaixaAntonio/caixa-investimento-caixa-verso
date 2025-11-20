using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Painel.Investimento.Domain.Models;

namespace Painel.investimento.Infra.Mappings.Configurations
{

    public class EnderecoConfiguration : IEntityTypeConfiguration<Endereco>
    {
        public void Configure(EntityTypeBuilder<Endereco> builder)
        {
            builder.ToTable("Endereco");
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Logradouro).HasMaxLength(150).IsRequired();
            builder.Property(e => e.Numero).HasMaxLength(10);
            builder.Property(e => e.Complemento).HasMaxLength(50);
            builder.Property(e => e.Bairro).HasMaxLength(100);
            builder.Property(e => e.Cidade).HasMaxLength(100);           
           
            
        }

    }

}


    


