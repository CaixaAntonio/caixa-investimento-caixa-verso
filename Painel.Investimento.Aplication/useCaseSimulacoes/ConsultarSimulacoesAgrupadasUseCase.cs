using Painel.Investimento.Domain.Dtos;
using Painel.Investimento.Domain.Repository.Abstract;

namespace Painel.Investimento.Aplication.useCaseSimulacoes
{
    public class ConsultarSimulacoesAgrupadasUseCase
    {
        private readonly ISimulacaoRepository _simulacaoRepo;

        public ConsultarSimulacoesAgrupadasUseCase(ISimulacaoRepository simulacaoRepo)
        {
            _simulacaoRepo = simulacaoRepo;
        }

        public async Task<IEnumerable<SimulacaoPorDiaProdutoResponse>> ExecuteAsync()
        {
            return await _simulacaoRepo.GetSimulacoesAgrupadasAsync();
        }
    }
}
