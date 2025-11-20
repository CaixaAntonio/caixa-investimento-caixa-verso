using Painel.Investimento.Domain.Models;
using Painel.Investimento.Domain.Repository.Abstract;

namespace Painel.Investimento.Aplication.UseCasesProdutos
{
    public class ProdutoInvestimentoUseCase
    {
        private readonly IProdutoInvestimentoRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public ProdutoInvestimentoUseCase(IProdutoInvestimentoRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        // ✅ Criar novo produto
        public async Task<ProdutoInvestimento> ExecuteAsync(
            string nome,
            string tipo,
            decimal rentabilidadeAnual,
            int risco,
            string liquidez,
            string tributacao,
            string garantia,
            string descricao)
        {
            var produto = new ProdutoInvestimento(
                nome,
                tipo,
                rentabilidadeAnual,
                risco,
                liquidez,
                tributacao,
                garantia,
                descricao
            );

            await _repository.AddAsync(produto);
            await _unitOfWork.CommitAsync();

            return produto;
        }

        public async Task<ProdutoInvestimento?> ObterPorIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        // ✅ Listar todos
        public async Task<IEnumerable<ProdutoInvestimento>> ListarTodosAsync()
        {
            return await _repository.GetAllAsync();
        }

        // ✅ Atualizar produto
        public async Task<ProdutoInvestimento?> AtualizarAsync(
            int id,
            string nome,
            string tipo,
            decimal? rentabilidadeAnual,
            int? risco,
            string liquidez,
            string tributacao,
            string garantia,
            string descricao)
        {
            var produto = await _repository.GetByIdAsync(id);
            if (produto == null) return null;

            produto.AtualizarProdutoInvestimento(
                nome,
                tipo,
                rentabilidadeAnual ?? produto.RentabilidadeAnual,
                risco ?? produto.Risco,
                liquidez,
                tributacao,
                garantia,
                descricao
            );

            await _unitOfWork.CommitAsync();
            return produto;
        }

        // ✅ Remover produto
        public async Task<bool> RemoverAsync(int id)
        {
            var produto = await _repository.GetByIdAsync(id);
            if (produto == null) return false;

            _repository.Remove(produto);
            await _unitOfWork.CommitAsync();
            return true;
        }
    }
}
