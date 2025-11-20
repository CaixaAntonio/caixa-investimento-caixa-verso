using Painel.Investimento.Domain.Models;

namespace Painel.Investimento.Domain.Repository.Abstract
{
    public interface IClienteRepository
    {
        Task AdicionarAsync(Cliente cliente);
        Task<Cliente?> ObterPorIdAsync(int id);
        Task<IEnumerable<Cliente>> ObterTodosAsync();
        void Remover(Cliente cliente);
    }
}
