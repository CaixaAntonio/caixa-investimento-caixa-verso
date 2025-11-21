using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Painel.investimento.Infra.Data;
using Painel.Investimento.Domain.Models;

public static class EnderecoSeeder
{
    public static async Task SeedEnderecosAsync(AppDbContext context)
    {
        // Garante que o banco está criado
        await context.Database.EnsureCreatedAsync();

        // Só insere se não houver endereços
        if (!await context.Enderecos.AnyAsync())
        {
            // Pegamos clientes já existentes para vincular
            var cliente1 = await context.Clientes.FirstOrDefaultAsync(c => c.Nome == "Antonio");
            var cliente2 = await context.Clientes.FirstOrDefaultAsync(c => c.Nome == "Maria");

            if (cliente1 != null)
            {
                var endereco1 = new Endereco(
                    logradouro: "Rua das Flores",
                    numero: "123",
                    complemento: "Apto 101",
                    bairro: "Centro",
                    cidade: "Belo Horizonte",
                    estado: "MG",
                    cep: "30123-456",
                    clienteId: cliente1.Id
                );

                context.Enderecos.Add(endereco1);
            }

            if (cliente2 != null)
            {
                var endereco2 = new Endereco(
                    logradouro: "Av. Brasil",
                    numero: "456",
                    complemento: "Casa",
                    bairro: "Funcionários",
                    cidade: "Belo Horizonte",
                    estado: "MG",
                    cep: "30145-789",
                    clienteId: cliente2.Id
                );

                context.Enderecos.Add(endereco2);
            }

            await context.SaveChangesAsync();
        }
    }
}
