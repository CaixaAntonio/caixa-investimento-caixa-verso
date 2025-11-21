using BCrypt.Net;
using Microsoft.AspNetCore.Identity;
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
                Role = "admin"
            };

            // Usa o mesmo PasswordHasher que o login
            var passwordHasher = new PasswordHasher<Usuario>();
            admin.PasswordHash = passwordHasher.HashPassword(admin, "123456");

            context.Usuarios.Add(admin);
            await context.SaveChangesAsync();
        }
    }

}
