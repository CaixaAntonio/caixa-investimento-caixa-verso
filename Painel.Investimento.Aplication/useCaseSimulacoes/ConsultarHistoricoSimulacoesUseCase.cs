using Painel.Investimento.Domain.Dtos;
using Painel.Investimento.Domain.Repository.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Painel.Investimento.Aplication.useCaseSimulacoes
{
    public class ConsultarHistoricoSimulacoesUseCase
    {
        private readonly ISimulacaoRepository _simulacaoRepo;

        public ConsultarHistoricoSimulacoesUseCase(ISimulacaoRepository simulacaoRepo)
        {
            _simulacaoRepo = simulacaoRepo;
        }

        public async Task<SimulacaoHistoricoResponse> ExecuteAsync(int clienteId)
        {
            var simulacoes = await _simulacaoRepo.GetByClienteIdAsync(clienteId);

            return new SimulacaoHistoricoResponse
            {
                ClienteId = clienteId,
                Simulacoes = simulacoes
            };
        }
    }

}
