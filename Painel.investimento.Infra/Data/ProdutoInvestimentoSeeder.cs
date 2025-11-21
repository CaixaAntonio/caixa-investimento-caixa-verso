using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Painel.investimento.Infra.Data;
using Painel.Investimento.Domain.Models;

public static class ProdutoInvestimentoSeeder
{
    public static async Task SeedProdutosAsync(AppDbContext context)
    {
        // Garante que o banco está criado
        await context.Database.EnsureCreatedAsync();

        if (!await context.ProdutosInvestimento.AnyAsync())
        {
            var cdb = new ProdutoInvestimento(
                nome: "CDB",
                tipoPerfil: "Conservador",
                rentabilidadeAnual: 0.08m, // 8% ao ano
                risco: 20,
                liquidez: "Diária",
                tributacao: "IR regressivo",
                garantia: "Fundo Garantidor de Créditos (FGC)",
                descricao: "Título de renda fixa emitido por bancos, indicado para perfis conservadores."
            );

            var lci = new ProdutoInvestimento(
                nome: "LCI",
                tipoPerfil: "Moderado",
                rentabilidadeAnual: 0.09m, // 9% ao ano
                risco: 40,
                liquidez: "90 dias",
                tributacao: "Isento de IR",
                garantia: "Fundo Garantidor de Créditos (FGC)",
                descricao: "Letra de Crédito Imobiliário, indicada para perfis moderados."
            );

            var acoes = new ProdutoInvestimento(
                nome: "Ações",
                tipoPerfil: "Arrojado",
                rentabilidadeAnual: 0.15m, // 15% ao ano
                risco: 80,
                liquidez: "Imediata (pregão)",
                tributacao: "15% sobre ganho de capital",
                garantia: "Sem garantia",
                descricao: "Investimento em ações de empresas listadas na bolsa, indicado para perfis arrojados."
            );

            var fundos = new ProdutoInvestimento(
                nome: "Fundos Multimercado",
                tipoPerfil: "Moderado/Arrojado",
                rentabilidadeAnual: 0.12m, // 12% ao ano
                risco: 60,
                liquidez: "D+30",
                tributacao: "IR regressivo",
                garantia: "Sem garantia",
                descricao: "Fundos que mesclam renda fixa e variável, indicados para perfis moderados e arrojados."
            );

            context.ProdutosInvestimento.AddRange(cdb, lci, acoes, fundos);
            await context.SaveChangesAsync();
        }
    }
}
