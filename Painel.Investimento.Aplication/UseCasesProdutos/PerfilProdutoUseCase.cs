using Painel.Investimento.Domain.Models;
using Painel.Investimento.Domain.Repository.Abstract;

namespace Painel.Investimento.Aplication.UseCasesProdutos
{
    public class PerfilProdutoUseCase
    {
        private readonly IPerfilProdutoRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public PerfilProdutoUseCase(IPerfilProdutoRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        // ✅ Criar vínculo entre PerfilDeRisco e ProdutoInvestimento
        public async Task<PerfilProduto> VincularAsync(int perfilDeRiscoId, int produtoInvestimentoId)
        {
            var perfilProduto = new PerfilProduto(perfilDeRiscoId, produtoInvestimentoId);

            await _repository.AddAsync(perfilProduto);
            await _unitOfWork.CommitAsync();

            return perfilProduto;
        }

        // ✅ Obter vínculo específico
        public async Task<PerfilProduto?> ObterPorIdsAsync(int perfilDeRiscoId, int produtoInvestimentoId)
        {
            return await _repository.GetByIdsAsync(perfilDeRiscoId, produtoInvestimentoId);
        }

        // ✅ Listar todos os vínculos
        public async Task<IEnumerable<PerfilProduto>> ListarTodosAsync()
        {
            return await _repository.GetAllAsync();
        }

        // ✅ Remover vínculo
        public async Task<bool> RemoverAsync(int perfilDeRiscoId, int produtoInvestimentoId)
        {
            var perfilProduto = await _repository.GetByIdsAsync(perfilDeRiscoId, produtoInvestimentoId);
            if (perfilProduto == null) return false;

            _repository.Remove(perfilProduto);
            await _unitOfWork.CommitAsync();
            return true;
        }
    }
}
