using Painel.Investimento.Domain.Models;

namespace Painel.Investimento.Domain.Repository.Abstract
{
    public interface IInvestimentosRepository
    {
        Task<Investimentos?> ObterPorIdAsync(int id);
        Task<IEnumerable<Investimentos>> ObterPorClienteAsync(int clienteId);
        Task<IEnumerable<Investimentos>> ObterInvestimentOldPorIdAsync(int clienteId, int produtoInvestID);
        Task<IEnumerable<Investimentos>> ObterTodosAsync();
        Task AdicionarAsync(Investimentos investimento);
        Task AtualizarAsync(Investimentos investimento);
        void RemoverAsync(int id);
    }
}
