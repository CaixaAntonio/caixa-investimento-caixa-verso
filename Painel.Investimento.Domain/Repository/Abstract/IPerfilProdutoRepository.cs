using Painel.Investimento.Domain.Models;

namespace Painel.Investimento.Domain.Repository.Abstract
{
    public interface IPerfilProdutoRepository
    {
        // ✅ Adicionar vínculo
        Task AddAsync(PerfilProduto perfilProduto);

        // ✅ Obter vínculo específico (pela chave composta)
        Task<PerfilProduto?> GetByIdsAsync(int perfilDeRiscoId, int produtoInvestimentoId);

        // ✅ Listar todos os vínculos
        Task<IEnumerable<PerfilProduto>> GetAllAsync();

        // ✅ Remover vínculo
        void Remove(PerfilProduto perfilProduto);
    }
}
