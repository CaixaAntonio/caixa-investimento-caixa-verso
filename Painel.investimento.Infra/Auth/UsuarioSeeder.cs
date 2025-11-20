using BCrypt.Net;
using Painel.investimento.Infra.Data;
using Painel.Investimento.Domain.Models;
using Painel.Investimento.Infra.Data;

public static class UsuarioSeeder
{
    public static async Task SeedAdminAsync(AppDbContext context)
    {
        if (!context.Usuarios.Any(u => u.Username == "admin"))
        {
            var admin = new Usuario
            {
                Username = "admin",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("123456"),
                Role = "admin"
            };

            context.Usuarios.Add(admin);
            await context.SaveChangesAsync();
        }
    }
}
