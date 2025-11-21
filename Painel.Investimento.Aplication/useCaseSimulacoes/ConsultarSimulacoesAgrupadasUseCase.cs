using Microsoft.Extensions.Logging;
using Painel.Investimento.Domain.Dtos;
using Painel.Investimento.Domain.Repository.Abstract;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Painel.Investimento.Aplication.useCaseSimulacoes
{
    public class ConsultarSimulacoesAgrupadasUseCase
    {
        private readonly ISimulacaoRepository _simulacaoRepo;
        private readonly ILogger<ConsultarSimulacoesAgrupadasUseCase> _logger;

        public ConsultarSimulacoesAgrupadasUseCase(
            ISimulacaoRepository simulacaoRepo,
            ILogger<ConsultarSimulacoesAgrupadasUseCase> logger)
        {
            _simulacaoRepo = simulacaoRepo;
            _logger = logger;
        }

        public async Task<IEnumerable<SimulacaoPorDiaProdutoResponse>> ExecuteAsync()
        {
            try
            {
                _logger.LogInformation("Consultando simulações agrupadas por dia e produto.");

                var simulacoes = await _simulacaoRepo.GetSimulacoesAgrupadasAsync();

                if (simulacoes == null)
                {
                    _logger.LogWarning("Nenhuma simulação agrupada encontrada.");
                    return new List<SimulacaoPorDiaProdutoResponse>();
                }

                _logger.LogInformation("Foram encontradas {Quantidade} simulações agrupadas.", simulacoes.Count());

                return simulacoes;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao consultar simulações agrupadas.");
                throw; // repropaga para ser tratado em nível superior (controller/middleware)
            }
        }
    }
}
