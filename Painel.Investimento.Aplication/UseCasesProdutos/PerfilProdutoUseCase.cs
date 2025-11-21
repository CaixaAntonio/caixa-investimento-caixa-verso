using Microsoft.Extensions.Logging;
using Painel.Investimento.Domain.Models;
using Painel.Investimento.Domain.Repository.Abstract;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Painel.Investimento.Aplication.UseCasesProdutos
{
    public class PerfilProdutoUseCase
    {
        private readonly IPerfilProdutoRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<PerfilProdutoUseCase> _logger;

        public PerfilProdutoUseCase(IPerfilProdutoRepository repository, IUnitOfWork unitOfWork, ILogger<PerfilProdutoUseCase> logger)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        // ✅ Criar vínculo entre PerfilDeRisco e ProdutoInvestimento
        public async Task<PerfilProduto> VincularAsync(int perfilDeRiscoId, int produtoInvestimentoId)
        {
            try
            {
                _logger.LogInformation("Criando vínculo entre PerfilDeRiscoId={PerfilDeRiscoId} e ProdutoInvestimentoId={ProdutoInvestimentoId}", perfilDeRiscoId, produtoInvestimentoId);

                var perfilProduto = new PerfilProduto(perfilDeRiscoId, produtoInvestimentoId);

                await _repository.AddAsync(perfilProduto);
                await _unitOfWork.CommitAsync();

                _logger.LogInformation("Vínculo criado com sucesso: PerfilProdutoId={PerfilProdutoId}", perfilProduto.PerfilDeRiscoId);

                return perfilProduto;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao criar vínculo entre PerfilDeRiscoId={PerfilDeRiscoId} e ProdutoInvestimentoId={ProdutoInvestimentoId}", perfilDeRiscoId, produtoInvestimentoId);
                throw;
            }
        }

        // ✅ Obter vínculo específico
        public async Task<PerfilProduto?> ObterPorIdsAsync(int perfilDeRiscoId, int produtoInvestimentoId)
        {
            try
            {
                _logger.LogInformation("Consultando vínculo entre PerfilDeRiscoId={PerfilDeRiscoId} e ProdutoInvestimentoId={ProdutoInvestimentoId}", perfilDeRiscoId, produtoInvestimentoId);

                var perfilProduto = await _repository.GetByIdsAsync(perfilDeRiscoId, produtoInvestimentoId);

                if (perfilProduto == null)
                {
                    _logger.LogWarning("Nenhum vínculo encontrado para PerfilDeRiscoId={PerfilDeRiscoId} e ProdutoInvestimentoId={ProdutoInvestimentoId}", perfilDeRiscoId, produtoInvestimentoId);
                }

                return perfilProduto;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao consultar vínculo entre PerfilDeRiscoId={PerfilDeRiscoId} e ProdutoInvestimentoId={ProdutoInvestimentoId}", perfilDeRiscoId, produtoInvestimentoId);
                throw;
            }
        }

        // ✅ Listar todos os vínculos
        public async Task<IEnumerable<PerfilProduto>> ListarTodosAsync()
        {
            try
            {
                _logger.LogInformation("Listando todos os vínculos PerfilProduto.");

                var lista = await _repository.GetAllAsync();

                _logger.LogInformation("Foram encontrados {Quantidade} vínculos PerfilProduto.", lista?.Count() ?? 0);

                return lista;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao listar vínculos PerfilProduto.");
                throw;
            }
        }

        // ✅ Remover vínculo
        public async Task<bool> RemoverAsync(int perfilDeRiscoId, int produtoInvestimentoId)
        {
            try
            {
                _logger.LogInformation("Removendo vínculo entre PerfilDeRiscoId={PerfilDeRiscoId} e ProdutoInvestimentoId={ProdutoInvestimentoId}", perfilDeRiscoId, produtoInvestimentoId);

                var perfilProduto = await _repository.GetByIdsAsync(perfilDeRiscoId, produtoInvestimentoId);
                if (perfilProduto == null)
                {
                    _logger.LogWarning("Vínculo não encontrado para remoção: PerfilDeRiscoId={PerfilDeRiscoId}, ProdutoInvestimentoId={ProdutoInvestimentoId}", perfilDeRiscoId, produtoInvestimentoId);
                    return false;
                }

                _repository.Remove(perfilProduto);
                await _unitOfWork.CommitAsync();

                _logger.LogInformation("Vínculo removido com sucesso: PerfilProdutoId={PerfilProdutoId}", perfilProduto.PerfilDeRiscoId);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao remover vínculo entre PerfilDeRiscoId={PerfilDeRiscoId} e ProdutoInvestimentoId={ProdutoInvestimentoId}", perfilDeRiscoId, produtoInvestimentoId);
                throw;
            }
        }
    }
}
