using Painel.Investimento.Domain.Models;
using Painel.Investimento.Domain.Repository.Abstract;
using Painel.Investimento.Domain.Services;

namespace Painel.Investimento.Aplication.UseCaseInvestimentos
{
    public class CalcularPerfilDeRiscoUseCase
    {
        private readonly IInvestimentosRepository _investimentoRepo;
        private readonly IPerfilDeRiscoRepository _perfilRepo;
        private readonly IRiskProfileService _riskService;
        public CalcularPerfilDeRiscoUseCase(
            IInvestimentosRepository investimentoRepo,
            IPerfilDeRiscoRepository perfilRepo,
            IRiskProfileService riskService)
        {
            _investimentoRepo = investimentoRepo;
            _perfilRepo = perfilRepo;
            _riskService = riskService;
        }

        public async Task<PerfilDeRisco> ExecuteAsync(int clienteId)
        {
            // 1. Buscar os investimentos do cliente
            var investimentos = await _investimentoRepo.ObterPorClienteAsync(clienteId);

            // 2. Calcular pontuação com base nos investimentos
            int pontuacao = CalcularPontuacao(investimentos);

            // 3. Buscar perfis disponíveis
            var perfis = await _perfilRepo.GetAllAsync();

            // 4. Determinar perfil correspondente
            var perfil = _riskService.DeterminarPerfil(pontuacao, perfis);

            return perfil;
        }

        public int CalcularPontuacao(IEnumerable<Investimentos> investimentos)
        {
            int pontuacao = 0;

            foreach (var inv in investimentos)
            {
                // Valor investido
                if (inv.ValorInvestido >= 500) pontuacao += 20;
                else if (inv.ValorInvestido >= 100) pontuacao += 10;
                else if (inv.ValorInvestido > 0) pontuacao += 5;

                // Prazo
                if (inv.PrazoMeses.HasValue)
                {
                    if (inv.PrazoMeses >= 24) pontuacao += 15;
                    else if (inv.PrazoMeses >= 12) pontuacao += 10;
                    else pontuacao += 5;
                }

                // Crise e retiradas
                if (inv.Crise)
                {
                    if (inv.ValorRetirado.HasValue && inv.ValorRetirado > 0)
                        pontuacao -= 15;
                    else
                        pontuacao -= 5;
                }
            }

            // Limitar entre 0 e 100
            if (pontuacao < 0) pontuacao = 0;
            if (pontuacao > 100) pontuacao = 100;

            return pontuacao;
        }
        public string ClassificarPerfil(int pontuacao)
        {
            if (pontuacao <= 35) return "Conservador, Perfil de baixo risco";
            if (pontuacao <= 65) return "Moderado, Perfil de risco moderado";
            return "Agressivo, Perfil de risco agressivo e alto investimento";
        }



    }
}
