using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Painel.Investimento.Domain.Models;
using Painel.Investimento.Domain.Valueobjects;

namespace Painel.investimento.Infra.Data
{
    public static class DatabaseSeeder
    {
        public static async Task EnsureDatabaseExistsAsync(IConfiguration configuration, AppDbContext context)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            // Extrai o nome do banco da connection string
            var builder = new SqlConnectionStringBuilder(connectionString);
            var PainelDb2 = builder.InitialCatalog;

            // Cria uma nova connection string sem o nome do banco
            builder.InitialCatalog = "master";
            var masterConnectionString = builder.ConnectionString;

            using var connection = new SqlConnection(masterConnectionString);
            await connection.OpenAsync();

            var command = connection.CreateCommand();
            command.CommandText = $@"
            IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'{PainelDb2}')
            BEGIN
                CREATE DATABASE [{PainelDb2}]
            END";

            await command.ExecuteNonQueryAsync();

            // Garante que o modelo está aplicado (cria tabela Cliente conforme ClienteConfiguration)
            await context.Database.MigrateAsync();

           
        }
    }
}
