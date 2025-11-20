namespace Painel.Investimento.Domain.Models
{
    public class Transacao
    {
        public int Id { get; private set; }
        public int ClienteId { get; private set; }
        public decimal Valor { get; private set; }
        public DateTime Data { get; private set; }
        public string Tipo { get; private set; } = string.Empty; // Ex.: "Gasto", "Investimento", "Renda"

        private Transacao() { } // EF Core

        public Transacao(int clienteId, decimal valor, DateTime data, string tipo)
        {
            ClienteId = clienteId;
            Valor = valor;
            Data = data;
            Tipo = tipo;
        }
    }
}
