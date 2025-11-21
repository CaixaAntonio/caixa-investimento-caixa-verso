using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Painel.investimento.Infra.Data;
using Painel.Investimento.Domain.Models;
using Painel.Investimento.Domain.Valueobjects;

public static class ClienteSeeder
{
    public static async Task SeedClientesAsync(AppDbContext context)
    {
        // Garante que o banco está criado
        await context.Database.EnsureCreatedAsync();

        if (!await context.Clientes.AnyAsync())
        {
            var cliente1 = new Cliente(
                id: 0, // EF Core vai gerar o Id
                nome: "Antonio",
                sobrenome: "Silva",
                email: new Email("antonio.silva@email.com"),
                celular: new Celular("31999999999"),
                cpf: new Cpf("12345678901"),
                dataDeNascimento: new DataDeNascimento(new DateTime(1990, 5, 10)),
                perfilDeRiscoId: 1
            );

            var cliente2 = new Cliente(
                id: 0,
                nome: "Maria",
                sobrenome: "Oliveira",
                email: new Email("maria.oliveira@email.com"),
                celular: new Celular("31988888888"),
                cpf: new Cpf("98765432100"),
                dataDeNascimento: new DataDeNascimento(new DateTime(1997, 3, 22)),
                perfilDeRiscoId: 2
            );

            context.Clientes.AddRange(cliente1, cliente2);
            await context.SaveChangesAsync();
        }
    }
}
