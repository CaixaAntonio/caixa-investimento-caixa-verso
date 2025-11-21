using Microsoft.Extensions.Logging;
using Painel.Investimento.Aplication.UseCaseInvestimentos;
using Painel.Investimento.Domain.Dtos;
using Painel.Investimento.Domain.Repository.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Painel.Investimento.Aplication.UseCasesProdutos
{
    public class RecomendarProdutosUseCase
    {
        private readonly CalcularPerfilDeRiscoUseCase _perfilUseCase;
        private readonly IProdutoInvestimentoRepository _produtoRepo;
        private readonly ILogger<RecomendarProdutosUseCase> _logger;

        public RecomendarProdutosUseCase(
            CalcularPerfilDeRiscoUseCase perfilUseCase,
            IProdutoInvestimentoRepository produtoRepo,
            ILogger<RecomendarProdutosUseCase> logger)
        {
            _perfilUseCase = perfilUseCase;
            _produtoRepo = produtoRepo;
            _logger = logger;
        }

        public async Task<IEnumerable<ProdutoRecomendadoDto>> ExecuteAsync(int clienteId)
        {
            try
            {
                _logger.LogInformation("Iniciando recomendação de produtos para ClienteId={ClienteId}", clienteId);

                // 1. Calcular perfil de risco do cliente
                var perfil = await _perfilUseCase.ExecuteAsync(clienteId);
                if (perfil == null)
                {
                    _logger.LogWarning("Perfil de risco não encontrado para ClienteId={ClienteId}", clienteId);
                    return Enumerable.Empty<ProdutoRecomendadoDto>();
                }

                _logger.LogInformation("Perfil de risco determinado: {Perfil}", perfil.Nome);

                // 2. Buscar todos os produtos disponíveis
                var produtos = await _produtoRepo.GetAllAsync();
                if (produtos == null || !produtos.Any())
                {
                    _logger.LogWarning("Nenhum produto de investimento disponível para recomendação.");
                    return Enumerable.Empty<ProdutoRecomendadoDto>();
                }

                // 3. Filtrar produtos de acordo com perfil
                var recomendados = produtos.Where(p =>
                    perfil.Nome == "Conservador" && p.Risco == 10 ||
                    perfil.Nome == "Moderado" && p.Risco == 20 ||
                    perfil.Nome == "Agressivo" && p.Risco == 30
                );

                if (!recomendados.Any())
                {
                    _logger.LogWarning("Nenhum produto recomendado para ClienteId={ClienteId} com perfil {Perfil}", clienteId, perfil.Nome);
                    return Enumerable.Empty<ProdutoRecomendadoDto>();
                }

                _logger.LogInformation("Foram recomendados {Quantidade} produtos para ClienteId={ClienteId}", recomendados.Count(), clienteId);

                // 4. Mapear para DTO
                return recomendados.Select(p => new ProdutoRecomendadoDto
                {
                    Id = (int)p.Id,
                    Nome = p.Nome,
                    Tipo = p.Tipo,
                    Rentabilidade = p.RentabilidadeAnual,
                    Risco = p.Risco switch
                    {
                        10 => "Baixo",
                        20 => "Médio",
                        30 => "Alto",
                        _ => "Desconhecido"
                    }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao recomendar produtos para ClienteId={ClienteId}", clienteId);
                throw;
            }
        }
    }
}
