using Painel.Investimento.Domain.Models;
using Painel.Investimento.Domain.Repository.Abstract;

namespace Painel.Investimento.Aplication.UseCaseInvestimentos
{
    public class InvestimentosUseCase
    {
        private readonly IInvestimentosRepository _repository;
        private readonly IClienteRepository _clienteRepository;
        private readonly IProdutoInvestimentoRepository _produtoRepository;
        private readonly IInvestimentosRepository _investimento;
        private readonly IUnitOfWork _unitOfWork;

        public InvestimentosUseCase(IInvestimentosRepository repository, IClienteRepository clienteRepository,
            IProdutoInvestimentoRepository produtoRepository, IInvestimentosRepository inestimento, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _clienteRepository = clienteRepository;
            _produtoRepository = produtoRepository;
            _investimento = inestimento;
            _unitOfWork = unitOfWork;
        }
        public async Task<Investimentos?> RegistrarAsync(
            int clienteId,
            int produtoInvestimentoId,
            decimal valorInvestido,
            DateTime dataInvestimento,
            int? prazoMeses
            )
        {
            // Valida cliente
            var cliente = await _clienteRepository.ObterPorIdAsync(clienteId);
            if (cliente == null)
                throw new ArgumentException("Cliente não encontrado.", nameof(clienteId));

            // Valida produto
            var produto = await _produtoRepository.ObterPorIdAsync(produtoInvestimentoId);
            if (produto == null)
                throw new ArgumentException("Produto de investimento não encontrado.", nameof(produtoInvestimentoId));

            // Cria entidade
            var investimento = new Investimentos(clienteId, produtoInvestimentoId, valorInvestido, dataInvestimento, prazoMeses);

            await _repository.AdicionarAsync(investimento);
            await _unitOfWork.CommitAsync();

            return investimento;
        }

        public async Task<Investimentos?> RetirarInvestimentoAsync(
            int clienteId,
            int produtoInvestimentoId,
            decimal valorInvestido,
            DateTime dataInvestimento,
            int? prazoMeses,
             decimal valorRetirado,
            bool crise
            )
        {
            // Valida cliente
            var cliente = await _clienteRepository.ObterPorIdAsync(clienteId);
            if (cliente == null)
                throw new ArgumentException("Cliente não encontrado.", nameof(clienteId));

            // Valida produto
            var produto = await _produtoRepository.ObterPorIdAsync(produtoInvestimentoId);
            if (produto == null)
                throw new ArgumentException("Produto de investimento não encontrado.", nameof(produtoInvestimentoId));

            // Valida Invesmento
            var investold = await _investimento.ObterInvestimentOldPorIdAsync(clienteId, produtoInvestimentoId);
            if (produto == null)
                throw new ArgumentException("Investimento não encontrado.", nameof(produtoInvestimentoId));


            // Cria entidade
            var investimento = new Investimentos(clienteId, produtoInvestimentoId, 0, 0, dataInvestimento,
                              crise, valorRetirado);

            await _repository.AdicionarAsync(investimento);
            await _unitOfWork.CommitAsync();

            return investimento;
        }

        public async Task<Investimentos?> ObterPorIdAsync(int id) => await _repository.ObterPorIdAsync(id);

        public async Task<IEnumerable<Investimentos>> ListarPorClienteAsync(int clienteId) =>
            await _repository.ObterPorClienteAsync(clienteId);
        public async Task<Investimentos?> AtualizarAsync(int id, decimal? novoValor, int? novoPrazoMeses)
        {
            var investimento = await _repository.ObterPorIdAsync(id);
            if (investimento == null) return null;

            if (novoValor.HasValue && novoValor.Value > 0)
                investimento.GetType().GetProperty("ValorInvestido")?.SetValue(investimento, novoValor);

            if (novoPrazoMeses.HasValue)
                investimento.GetType().GetProperty("PrazoMeses")?.SetValue(investimento, novoPrazoMeses);

            await _unitOfWork.CommitAsync();
            return investimento;
        }

        public decimal CalcularRentabilidade(decimal ValorInvestido, decimal ValorRetirado)
        {
            if (ValorInvestido != 0 && ValorInvestido > 0)
            {
                var ganho = ValorRetirado - ValorInvestido;
                return ganho / ValorInvestido;
            }
            return 0;
        }
        public async Task<bool> RemoverAsync(int id)
        {
            var investimento = await _repository.ObterPorIdAsync(id);
            if (investimento == null) return false;

            _repository.RemoverAsync(id);
            await _unitOfWork.CommitAsync();
            return true;
        }
    }
}
