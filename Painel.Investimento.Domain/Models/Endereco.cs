namespace Painel.Investimento.Domain.Models
{
    public sealed class Endereco
    {
        public int Id { get; private set; }
        public string Logradouro { get; private set; }
        public string Numero { get; private set; }
        public string Complemento { get; private set; }
        public string Bairro { get; private set; }
        public string Cidade { get; private set; }
        public string Estado { get; private set; }
        public string Cep { get; private set; }

        public int ClienteId { get; private set; }
        public Cliente Cliente { get; private set; }

        private Endereco() { } // EF Core

        public Endereco(string logradouro, string numero, string complemento,
                        string bairro, string cidade, string estado, string cep, int clienteId)
        {
            Logradouro = logradouro;
            Numero = numero;
            Complemento = complemento;
            Bairro = bairro;
            Cidade = cidade;
            Estado = estado;
            Cep = cep;
            ClienteId = clienteId;
        }

        // ✅ Método para atualização
        public void Atualizar(string logradouro, string numero, string complemento,
                              string bairro, string cidade, string estado, string cep)
        {
            Logradouro = logradouro;
            Numero = numero;
            Complemento = complemento;
            Bairro = bairro;
            Cidade = cidade;
            Estado = estado;
            Cep = cep;
        }
    }
}
