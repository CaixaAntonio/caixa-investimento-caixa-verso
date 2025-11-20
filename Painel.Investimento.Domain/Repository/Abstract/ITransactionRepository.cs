using Painel.Investimento.Domain.Models;

namespace Painel.Investimento.Domain.Repository.Abstract
{
    public interface ITransactionRepository
    {
        /// <summary>
        /// Retorna todas as transações de um cliente específico.
        /// </summary>
        Task<IEnumerable<Transacao>> GetByClienteAsync(int clienteId);

        /// <summary>
        /// Retorna uma transação pelo seu Id.
        /// </summary>
        Task<Transacao?> GetByIdAsync(int id);

        /// <summary>
        /// Adiciona uma nova transação.
        /// </summary>
        Task AddAsync(Transacao transacao);

        /// <summary>
        /// Atualiza uma transação existente.
        /// </summary>
        Task UpdateAsync(Transacao transacao);

        /// <summary>
        /// Remove uma transação.
        /// </summary>
        Task DeleteAsync(int id);
    }
}
