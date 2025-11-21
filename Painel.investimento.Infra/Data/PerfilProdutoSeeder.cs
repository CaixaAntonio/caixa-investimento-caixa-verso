using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Painel.investimento.Infra.Data;
using Painel.Investimento.Domain.Models;

public static class PerfilProdutoSeeder
{
    public static async Task SeedPerfilProdutosAsync(AppDbContext context)
    {
        // Garante que o banco está criado
        await context.Database.EnsureCreatedAsync();

        // Só insere se não houver vínculos
        if (!await context.PerfilProdutos.AnyAsync())
        {
            // Busca perfis de risco e produtos já existentes
            var conservador = await context.PerfilDeRisco.FirstOrDefaultAsync(p => p.Nome == "Conservador");
            var moderado = await context.PerfilDeRisco.FirstOrDefaultAsync(p => p.Nome == "Moderado");
            var arrojado = await context.PerfilDeRisco.FirstOrDefaultAsync(p => p.Nome == "Arrojado");

            var produto1 = await context.ProdutosInvestimento.FirstOrDefaultAsync();
            var produto2 = await context.ProdutosInvestimento.Skip(1).FirstOrDefaultAsync();
            var produto3 = await context.ProdutosInvestimento.Skip(2).FirstOrDefaultAsync();

            var perfilProdutos = new List<PerfilProduto>();

            if (conservador != null && produto1 != null)
                perfilProdutos.Add(new PerfilProduto(conservador.Id, produto1.Id??0));

            if (moderado != null && produto2 != null)
                perfilProdutos.Add(new PerfilProduto(moderado.Id, produto2.Id ?? 0));

            if (arrojado != null && produto3 != null)
                perfilProdutos.Add(new PerfilProduto(arrojado.Id, produto3.Id ?? 0));

            if (perfilProdutos.Any())
            {
                context.PerfilProdutos.AddRange(perfilProdutos);
                await context.SaveChangesAsync();
            }
        }
    }
}
