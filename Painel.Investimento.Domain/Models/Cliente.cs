

using Painel.Investimento.Domain.Repository.Abstract;
using Painel.Investimento.Domain.Valueobjects;

namespace Painel.Investimento.Domain.Models
{
    public sealed class Cliente : IAggregateRoot
    {
        public int Id { get; private set; }
        public string Nome { get; private set; }
        public string Sobrenome { get; private set; }
        public Email Email { get; private set; }
        public Celular Celular { get; private set; }
        public Cpf Cpf { get; private set; }
        public Idade Idade { get; private set; }
        public DataDeNascimento DataDeNascimento { get; private set; }

        public int PerfilDeRiscoId { get; private set; }

        public ICollection<Endereco> Enderecos { get; private set; } = [];
        public ICollection<Investimentos> Investimentos { get; private set; } = [];

        private Cliente() { } // EF Core

        public Cliente(int id, string nome, string sobrenome, Email email, Celular celular, Cpf cpf,
                       DataDeNascimento dataDeNascimento, int perfilDeRiscoId)
        {
            if (string.IsNullOrWhiteSpace(nome))
                throw new ArgumentException("Nome não pode ser vazio.", nameof(nome));

            if (string.IsNullOrWhiteSpace(sobrenome))
                throw new ArgumentException("Sobrenome não pode ser vazio.", nameof(sobrenome));

            Id = id;
            Nome = nome;
            Sobrenome = sobrenome;
            Email = email ?? throw new ArgumentNullException(nameof(email));
            Celular = celular ?? throw new ArgumentNullException(nameof(celular));
            Cpf = cpf ?? throw new ArgumentNullException(nameof(cpf));
            DataDeNascimento = dataDeNascimento ?? throw new ArgumentNullException(nameof(dataDeNascimento));
            Idade = new Idade(dataDeNascimento.CalcularIdade());

            Investimentos = new List<Investimentos>();

            PerfilDeRiscoId = perfilDeRiscoId;
        }

        // Método de domínio para adicionar investimento
        public void AdicionarInvestimento(Investimentos investimento)
        {
            if (investimento == null)
                throw new ArgumentNullException(nameof(investimento));

            Investimentos.Add(investimento);
        }

        public void AdicionarEndereco(Endereco endereco)
        {
            if (endereco == null) throw new ArgumentNullException(nameof(endereco));
            Enderecos.Add(endereco);
        }

        public void AjustarPerfil(int novoPerfilDeRiscoId)
        {
            if (novoPerfilDeRiscoId <= 0)
                throw new ArgumentException("Perfil de risco inválido.", nameof(novoPerfilDeRiscoId));

            PerfilDeRiscoId = novoPerfilDeRiscoId;
        }

        public void AtualizarEndereco(int enderecoId, string logradouro, string numero, string complemento,
                                      string bairro, string cidade, string estado, string cep)
        {
            var endereco = Enderecos.FirstOrDefault(e => e.Id == enderecoId);
            if (endereco == null) throw new InvalidOperationException("Endereço não encontrado.");

            endereco.Atualizar(logradouro, numero, complemento, bairro, cidade, estado, cep);
        }

        public void RemoverEndereco(int enderecoId)
        {
            var endereco = Enderecos.FirstOrDefault(e => e.Id == enderecoId);
            if (endereco != null)
                Enderecos.Remove(endereco);
        }


    }
}
