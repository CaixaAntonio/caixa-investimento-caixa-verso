namespace Painel.Investimento.Domain.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty; // ⚠️ nunca armazene senha em texto puro
        public string Role { get; set; } = "user";
    }
}
