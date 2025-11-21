using Microsoft.Extensions.Logging;
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
        private readonly ILogger<CalcularPerfilDeRiscoUseCase> _logger;

        public CalcularPerfilDeRiscoUseCase(
            IInvestimentosRepository investimentoRepo,
            IPerfilDeRiscoRepository perfilRepo,
            IRiskProfileService riskService,
            ILogger<CalcularPerfilDeRiscoUseCase> logger)
        {
            _investimentoRepo = investimentoRepo;
            _perfilRepo = perfilRepo;
            _riskService = riskService;
            _logger = logger;
        }

        public async Task<PerfilDeRisco?> ExecuteAsync(int clienteId)
        {
            try
            {
                _logger.LogInformation("Iniciando cálculo de perfil de risco para ClienteId={ClienteId}", clienteId);

                // 1. Buscar os investimentos do cliente
                var investimentos = await _investimentoRepo.ObterPorClienteAsync(clienteId);
                if (investimentos == null || !investimentos.Any())
                {
                    _logger.LogWarning("Nenhum investimento encontrado para ClienteId={ClienteId}", clienteId);
                    return null;
                }

                // 2. Calcular pontuação com base nos investimentos
                int pontuacao = CalcularPontuacao(investimentos);
                _logger.LogInformation("Pontuação calculada para ClienteId={ClienteId}: {Pontuacao}", clienteId, pontuacao);

                // 3. Buscar perfis disponíveis
                var perfis = await _perfilRepo.GetAllAsync();
                if (perfis == null || !perfis.Any())
                {
                    _logger.LogError("Nenhum perfil de risco disponível no repositório.");
                    return null;
                }

                // 4. Determinar perfil correspondente
                var perfil = _riskService.DeterminarPerfil(pontuacao, perfis);
                _logger.LogInformation("Perfil de risco determinado para ClienteId={ClienteId}: {Perfil}", clienteId, perfil?.Descricao);

                return perfil;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao calcular perfil de risco para ClienteId={ClienteId}", clienteId);
                throw; // repropaga para ser tratado em nível superior
            }
        }

        public int CalcularPontuacao(IEnumerable<Investimentos> investimentos)
        {
            int pontuacao = 0;

            try
            {
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

                _logger.LogDebug("Pontuação final calculada: {Pontuacao}", pontuacao);

                return pontuacao;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao calcular pontuação de investimentos.");
                throw;
            }
        }

        public string ClassificarPerfil(int pontuacao)
        {
            try
            {
                string perfil;
                if (pontuacao <= 35) perfil = "Conservador, Perfil de baixo risco";
                else if (pontuacao <= 65) perfil = "Moderado, Perfil de risco moderado";
                else perfil = "Agressivo, Perfil de risco agressivo e alto investimento";

                _logger.LogInformation("Classificação de perfil para pontuação={Pontuacao}: {Perfil}", pontuacao, perfil);

                return perfil;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao classificar perfil com pontuação={Pontuacao}", pontuacao);
                throw;
            }
        }
    }
}
