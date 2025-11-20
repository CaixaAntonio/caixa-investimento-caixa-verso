namespace Painel.Investimento.Domain.Dtos
{
    public class EnderecoResponse
    {
        public int Id { get; set; }
        public string Logradouro { get; set; }
        public string Numero { get; set; }
        public string Complemento { get; set; }
        public string Bairro { get; set; }
        public string Cidade { get; set; }
        public string Estado { get; set; }
        public string Cep { get; set; }
    }

    public class ClienteResponse
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Sobrenome { get; set; }
        public string Email { get; set; }
        public string Celular { get; set; }
        public string Cpf { get; set; }
        public DateTime DataDeNascimento { get; set; }
        public int PerfilDeRiscoId { get; set; }

        public List<EnderecoResponse> Enderecos { get; set; }
    }

    public class EnderecoRequest
    {
        public string Logradouro { get; set; }
        public string Numero { get; set; }
        public string Complemento { get; set; }
        public string Bairro { get; set; }
        public string Cidade { get; set; }

        // Pode ser sigla (ex: "MG") ou nome completo, conforme você definiu no domínio
        public string Estado { get; set; }

        // Apenas números, 8 caracteres (ex: "30140071")
        public string Cep { get; set; }
    }


}
