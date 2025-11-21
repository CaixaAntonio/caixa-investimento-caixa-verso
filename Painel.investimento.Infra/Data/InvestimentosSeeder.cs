using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Painel.investimento.Infra.Data;
using Painel.Investimento.Domain.Models;

public static class InvestimentosSeeder
{
    public static async Task SeedInvestimentosAsync(AppDbContext context)
    {
        // Garante que o banco está criado
        await context.Database.EnsureCreatedAsync();

        // Só insere se não houver investimentos
        if (!await context.Investimentos.AnyAsync())
        {
            // Busca clientes e produtos já existentes
            var cliente1 = await context.Clientes.FirstOrDefaultAsync(c => c.Nome == "Antonio");
            var cliente2 = await context.Clientes.FirstOrDefaultAsync(c => c.Nome == "Maria");

            var produto1 = await context.ProdutosInvestimento.FirstOrDefaultAsync();
            var produto2 = await context.ProdutosInvestimento.Skip(1).FirstOrDefaultAsync();

            if (cliente1 != null && produto1 != null)
            {
                var investimento1 = new Investimentos(
                    clienteId: cliente1.Id,
                    produtoInvestimentoId: produto1.Id??0,
                    valorInvestido: 10000m,
                    dataInvestimento: DateTime.UtcNow.AddMonths(-6),
                    prazoMeses: 12
                );

                context.Investimentos.Add(investimento1);
            }

            if (cliente2 != null && produto2 != null)
            {
                var investimento2 = new Investimentos(
                    clienteId: cliente2.Id,
                    produtoInvestimentoId: produto2.Id??0,
                    valorInvestido: 5000m,
                    prazoMeses: 6,
                    dataInvestimento: DateTime.UtcNow.AddMonths(-3),
                    crise: false,
                    valorRetirado: null
                );

                context.Investimentos.Add(investimento2);
            }

            await context.SaveChangesAsync();
        }
    }
}
