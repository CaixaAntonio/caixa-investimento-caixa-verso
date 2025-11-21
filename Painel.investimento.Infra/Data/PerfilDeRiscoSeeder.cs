using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Painel.investimento.Infra.Data;
using Painel.Investimento.Domain.Models;

public static class PerfilDeRiscoSeeder
{
    public static async Task SeedPerfisDeRiscoAsync(AppDbContext context)
    {
        // Garante que o banco está criado
        await context.Database.EnsureCreatedAsync();

        if (!await context.PerfilDeRisco.AnyAsync())
        {
            var conservador = new PerfilDeRisco(
                nome: "Conservador",
                pontuacaoMinima: 0,
                pontuacaoMaxima: 30,
                descricao: "Perfil conservador: baixa tolerância ao risco, busca segurança e liquidez."
            );

            var moderado = new PerfilDeRisco(
                nome: "Moderado",
                pontuacaoMinima: 31,
                pontuacaoMaxima: 70,
                descricao: "Perfil moderado: aceita algum risco em troca de retornos melhores."
            );

            var arrojado = new PerfilDeRisco(
                nome: "Arrojado",
                pontuacaoMinima: 71,
                pontuacaoMaxima: 100,
                descricao: "Perfil arrojado: alta tolerância ao risco, busca maiores retornos."
            );

            context.PerfilDeRisco.AddRange(conservador, moderado, arrojado);
            await context.SaveChangesAsync();
        }
    }
}
