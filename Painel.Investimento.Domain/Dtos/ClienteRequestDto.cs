

namespace Painel.Investimento.Domain.Dtos
{
    public class ClienteRequestDto
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Sobrenome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Celular { get; set; } = string.Empty;
        public string Cpf { get; set; } = string.Empty;
        public DateTime DataDeNascimento { get; set; }

        public int PerfilDeRiscoId { get; set; }

    }


    public class ClienteResponseDto
    {
        public int Id { get; set; }
        public string NomeCompleto { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Celular { get; set; } = string.Empty;
        public string Cpf { get; set; } = string.Empty;
        public int Idade { get; set; }
        public DateTime DataDeNascimento { get; set; }
        public int PerfilDeRiscoId { get; set; }

    }
}