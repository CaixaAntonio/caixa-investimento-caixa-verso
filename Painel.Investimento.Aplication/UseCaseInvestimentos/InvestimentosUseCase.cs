using Microsoft.Extensions.Logging;
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
        private readonly ILogger<InvestimentosUseCase> _logger;

        public InvestimentosUseCase(
            IInvestimentosRepository repository,
            IClienteRepository clienteRepository,
            IProdutoInvestimentoRepository produtoRepository,
            IInvestimentosRepository investimento,
            IUnitOfWork unitOfWork,
            ILogger<InvestimentosUseCase> logger)
        {
            _repository = repository;
            _clienteRepository = clienteRepository;
            _produtoRepository = produtoRepository;
            _investimento = investimento;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Investimentos?> RegistrarAsync(
            int clienteId,
            int produtoInvestimentoId,
            decimal valorInvestido,
            DateTime dataInvestimento,
            int? prazoMeses)
        {
            try
            {
                _logger.LogInformation("Registrando investimento para ClienteId={ClienteId}, ProdutoId={ProdutoId}", clienteId, produtoInvestimentoId);

                var cliente = await _clienteRepository.ObterPorIdAsync(clienteId);
                if (cliente == null)
                {
                    _logger.LogWarning("Cliente não encontrado: {ClienteId}", clienteId);
                    throw new ArgumentException("Cliente não encontrado.", nameof(clienteId));
                }

                var produto = await _produtoRepository.ObterPorIdAsync(produtoInvestimentoId);
                if (produto == null)
                {
                    _logger.LogWarning("Produto de investimento não encontrado: {ProdutoId}", produtoInvestimentoId);
                    throw new ArgumentException("Produto de investimento não encontrado.", nameof(produtoInvestimentoId));
                }

                var investimento = new Investimentos(clienteId, produtoInvestimentoId, valorInvestido, dataInvestimento, prazoMeses);

                await _repository.AdicionarAsync(investimento);
                await _unitOfWork.CommitAsync();

                _logger.LogInformation("Investimento registrado com sucesso: Id={InvestimentoId}", investimento.Id);

                return investimento;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao registrar investimento para ClienteId={ClienteId}", clienteId);
                throw;
            }
        }

        public async Task<Investimentos?> RetirarInvestimentoAsync(
            int clienteId,
            int produtoInvestimentoId,
            decimal valorInvestido,
            DateTime dataInvestimento,
            int? prazoMeses,
            decimal valorRetirado,
            bool crise)
        {
            try
            {
                _logger.LogInformation("Retirando investimento para ClienteId={ClienteId}, ProdutoId={ProdutoId}", clienteId, produtoInvestimentoId);

                var cliente = await _clienteRepository.ObterPorIdAsync(clienteId);
                if (cliente == null)
                {
                    _logger.LogWarning("Cliente não encontrado: {ClienteId}", clienteId);
                    throw new ArgumentException("Cliente não encontrado.", nameof(clienteId));
                }

                var produto = await _produtoRepository.ObterPorIdAsync(produtoInvestimentoId);
                if (produto == null)
                {
                    _logger.LogWarning("Produto de investimento não encontrado: {ProdutoId}", produtoInvestimentoId);
                    throw new ArgumentException("Produto de investimento não encontrado.", nameof(produtoInvestimentoId));
                }

                var investold = await _investimento.ObterInvestimentOldPorIdAsync(clienteId, produtoInvestimentoId);
                if (investold == null)
                {
                    _logger.LogWarning("Investimento antigo não encontrado para ClienteId={ClienteId}, ProdutoId={ProdutoId}", clienteId, produtoInvestimentoId);
                    throw new ArgumentException("Investimento não encontrado.", nameof(produtoInvestimentoId));
                }

                var investimento = new Investimentos(clienteId, produtoInvestimentoId, 0, 0, dataInvestimento, crise, valorRetirado);

                await _repository.AdicionarAsync(investimento);
                await _unitOfWork.CommitAsync();

                _logger.LogInformation("Retirada registrada com sucesso para ClienteId={ClienteId}, ProdutoId={ProdutoId}", clienteId, produtoInvestimentoId);

                return investimento;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao retirar investimento para ClienteId={ClienteId}", clienteId);
                throw;
            }
        }

        public async Task<Investimentos?> ObterPorIdAsync(int id)
        {
            _logger.LogInformation("Consultando investimento por Id={Id}", id);
            return await _repository.ObterPorIdAsync(id);
        }

        public async Task<IEnumerable<Investimentos>> ListarPorClienteAsync(int clienteId)
        {
            _logger.LogInformation("Listando investimentos para ClienteId={ClienteId}", clienteId);
            return await _repository.ObterPorClienteAsync(clienteId);
        }

        public async Task<Investimentos?> AtualizarAsync(int id, decimal? novoValor, int? novoPrazoMeses)
        {
            try
            {
                _logger.LogInformation("Atualizando investimento Id={Id}", id);

                var investimento = await _repository.ObterPorIdAsync(id);
                if (investimento == null)
                {
                    _logger.LogWarning("Investimento não encontrado para atualização: Id={Id}", id);
                    return null;
                }

                if (novoValor.HasValue && novoValor.Value > 0)
                    investimento.GetType().GetProperty("ValorInvestido")?.SetValue(investimento, novoValor);

                if (novoPrazoMeses.HasValue)
                    investimento.GetType().GetProperty("PrazoMeses")?.SetValue(investimento, novoPrazoMeses);

                await _unitOfWork.CommitAsync();

                _logger.LogInformation("Investimento atualizado com sucesso: Id={Id}", id);

                return investimento;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao atualizar investimento Id={Id}", id);
                throw;
            }
        }

        public decimal CalcularRentabilidade(decimal valorInvestido, decimal valorRetirado)
        {
            try
            {
                if (valorInvestido > 0)
                {
                    var ganho = valorRetirado - valorInvestido;
                    var rentabilidade = ganho / valorInvestido;

                    _logger.LogInformation("Rentabilidade calculada: Investido={Investido}, Retirado={Retirado}, Rentabilidade={Rentabilidade}",
                        valorInvestido, valorRetirado, rentabilidade);

                    return rentabilidade;
                }

                _logger.LogWarning("Tentativa de calcular rentabilidade com valor investido inválido: {ValorInvestido}", valorInvestido);
                return 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao calcular rentabilidade.");
                throw;
            }
        }

        public async Task<bool> RemoverAsync(int id)
        {
            try
            {
                _logger.LogInformation("Removendo investimento Id={Id}", id);

                var investimento = await _repository.ObterPorIdAsync(id);
                if (investimento == null)
                {
                    _logger.LogWarning("Investimento não encontrado para remoção: Id={Id}", id);
                    return false;
                }

                _repository.RemoverAsync(id);
                await _unitOfWork.CommitAsync();

                _logger.LogInformation("Investimento removido com sucesso: Id={Id}", id);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao remover investimento Id={Id}", id);
                throw;
            }
        }
    }
}
