using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Painel.investimento.Infra.Data;
using Painel.Investimento.Domain.Services;
using Painel.Investimento.Infra.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Painel.Investimento.Infra.Auth
{
    public class JwtAuthService : IAuthService
    {
        private readonly IConfiguration _config;
        private readonly AppDbContext _context;

        public JwtAuthService(IConfiguration config, AppDbContext context)
        {
            _config = config;
            _context = context;
        }

        public async Task<bool> ValidateCredentialsAsync(string username, string password)
        {
            // ⚠️ Exemplo simples: valida usuário no banco
            var user = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Username == username && u.PasswordHash == password);

            return user != null;
        }

        public async Task<string> GenerateTokenAsync(string username, string role)
        {
            var jwt = _config.GetSection("Jwt");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt["Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, username),
                new Claim("role", role),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: jwt["Issuer"],
                audience: jwt["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: creds
            );

            return await Task.FromResult(new JwtSecurityTokenHandler().WriteToken(token));
        }
    }
}
