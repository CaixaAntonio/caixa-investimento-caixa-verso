using System;

namespace Painel.Investimento.Domain.Models
{
    public class PerfilDeRisco
    {
        public int Id { get; private set; }
        public string Nome { get; private set; }
        public int PontuacaoMinima { get; private set; }
        public int PontuacaoMaxima { get; private set; }
        public string Descricao { get; private set; }

        public ICollection<PerfilProduto> PerfilProdutos { get; private set; } = new List<PerfilProduto>();

        private PerfilDeRisco() { } // EF Core

        public PerfilDeRisco(int id, string nome, int pontuacaoMinima, int pontuacaoMaxima, string descricao)
        {
            AtualizarPerfil(id, nome, pontuacaoMinima, pontuacaoMaxima, descricao);
        }

        public void AtualizarPerfil(int id, string nome, int pontuacaoMinima, int pontuacaoMaxima, string descricao)
        {
            if (string.IsNullOrWhiteSpace(nome))
                throw new ArgumentException("Nome do perfil não pode ser vazio.", nameof(nome));

            if (pontuacaoMinima < 0)
                throw new ArgumentException("Pontuação mínima não pode ser negativa.", nameof(pontuacaoMinima));

            if (pontuacaoMaxima <= pontuacaoMinima)
                throw new ArgumentException("Pontuação máxima deve ser maior que a mínima.", nameof(pontuacaoMaxima));

            if (string.IsNullOrWhiteSpace(descricao))
                throw new ArgumentException("Descrição não pode ser vazia.", nameof(descricao));

            Id = id;
            Nome = nome;
            PontuacaoMinima = pontuacaoMinima;
            PontuacaoMaxima = pontuacaoMaxima;
            Descricao = descricao;
        }

        public bool PertenceAoIntervalo(int pontuacao)
        {
            return pontuacao >= PontuacaoMinima && pontuacao <= PontuacaoMaxima;
        }

        public void Validar()
        {
            if (string.IsNullOrWhiteSpace(Nome))
                throw new InvalidOperationException("Perfil inválido: Nome obrigatório.");
            if (PontuacaoMinima < 0)
                throw new InvalidOperationException("Perfil inválido: Pontuação mínima não pode ser negativa.");
            if (PontuacaoMaxima <= PontuacaoMinima)
                throw new InvalidOperationException("Perfil inválido: Pontuação máxima deve ser maior que a mínima.");
        }
    }
}
