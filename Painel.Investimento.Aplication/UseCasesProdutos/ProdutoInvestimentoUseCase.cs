using Microsoft.Extensions.Logging;
using Painel.Investimento.Domain.Models;
using Painel.Investimento.Domain.Repository.Abstract;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Painel.Investimento.Aplication.UseCasesProdutos
{
    public class ProdutoInvestimentoUseCase
    {
        private readonly IProdutoInvestimentoRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ProdutoInvestimentoUseCase> _logger;

        public ProdutoInvestimentoUseCase(
            IProdutoInvestimentoRepository repository,
            IUnitOfWork unitOfWork,
            ILogger<ProdutoInvestimentoUseCase> logger)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _logger = logger;
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
            try
            {
                _logger.LogInformation("Criando novo produto de investimento: {Nome}, Tipo={Tipo}", nome, tipo);

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

                _logger.LogInformation("Produto criado com sucesso: Id={Id}, Nome={Nome}", produto.Id, produto.Nome);

                return produto;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao criar produto de investimento: {Nome}, Tipo={Tipo}", nome, tipo);
                throw;
            }
        }

        public async Task<ProdutoInvestimento?> ObterPorIdAsync(int id)
        {
            try
            {
                _logger.LogInformation("Consultando produto de investimento Id={Id}", id);

                var produto = await _repository.GetByIdAsync(id);

                if (produto == null)
                {
                    _logger.LogWarning("Produto de investimento não encontrado: Id={Id}", id);
                }

                return produto;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao consultar produto de investimento Id={Id}", id);
                throw;
            }
        }

        // ✅ Listar todos
        public async Task<IEnumerable<ProdutoInvestimento>> ListarTodosAsync()
        {
            try
            {
                _logger.LogInformation("Listando todos os produtos de investimento.");

                var lista = await _repository.GetAllAsync();

                _logger.LogInformation("Foram encontrados {Quantidade} produtos de investimento.", lista?.Count() ?? 0);

                return lista;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao listar produtos de investimento.");
                throw;
            }
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
            try
            {
                _logger.LogInformation("Atualizando produto de investimento Id={Id}", id);

                var produto = await _repository.GetByIdAsync(id);
                if (produto == null)
                {
                    _logger.LogWarning("Produto de investimento não encontrado para atualização: Id={Id}", id);
                    return null;
                }

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

                _logger.LogInformation("Produto atualizado com sucesso: Id={Id}, Nome={Nome}", produto.Id, produto.Nome);

                return produto;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao atualizar produto de investimento Id={Id}", id);
                throw;
            }
        }

        // ✅ Remover produto
        public async Task<bool> RemoverAsync(int id)
        {
            try
            {
                _logger.LogInformation("Removendo produto de investimento Id={Id}", id);

                var produto = await _repository.GetByIdAsync(id);
                if (produto == null)
                {
                    _logger.LogWarning("Produto de investimento não encontrado para remoção: Id={Id}", id);
                    return false;
                }

                _repository.Remove(produto);
                await _unitOfWork.CommitAsync();

                _logger.LogInformation("Produto removido com sucesso: Id={Id}", id);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao remover produto de investimento Id={Id}", id);
                throw;
            }
        }
    }
}
