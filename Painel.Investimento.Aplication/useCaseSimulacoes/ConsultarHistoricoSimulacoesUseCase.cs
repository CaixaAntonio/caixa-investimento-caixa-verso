using Microsoft.Extensions.Logging;
using Painel.Investimento.Domain.Dtos;
using Painel.Investimento.Domain.Repository.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Painel.Investimento.Aplication.useCaseSimulacoes
{
    public class ConsultarHistoricoSimulacoesUseCase
    {
        private readonly ISimulacaoRepository _simulacaoRepo;
        private readonly ILogger<ConsultarHistoricoSimulacoesUseCase> _logger;

        public ConsultarHistoricoSimulacoesUseCase(
            ISimulacaoRepository simulacaoRepo,
            ILogger<ConsultarHistoricoSimulacoesUseCase> logger)
        {
            _simulacaoRepo = simulacaoRepo;
            _logger = logger;
        }

        public async Task<SimulacaoHistoricoResponse> ExecuteAsync(int clienteId)
        {
            try
            {
                _logger.LogInformation("Consultando histórico de simulações para ClienteId={ClienteId}", clienteId);

                var simulacoes = await _simulacaoRepo.GetByClienteIdAsync(clienteId);

                if (simulacoes == null || !simulacoes.Any())
                {
                    _logger.LogWarning("Nenhuma simulação encontrada para ClienteId={ClienteId}", clienteId);
                }
                else
                {
                    _logger.LogInformation("Foram encontradas {Quantidade} simulações para ClienteId={ClienteId}", simulacoes.Count(), clienteId);
                }

                return new SimulacaoHistoricoResponse
                {
                    ClienteId = clienteId,
                    Simulacoes = simulacoes
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao consultar histórico de simulações para ClienteId={ClienteId}", clienteId);
                throw; // repropaga para ser tratado em nível superior (controller/middleware)
            }
        }
    }
}
