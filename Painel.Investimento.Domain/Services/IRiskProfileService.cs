using Painel.Investimento.Domain.Models;

namespace Painel.Investimento.Domain.Services
{
    public interface IRiskProfileService
    {
        PerfilDeRisco DeterminarPerfil(int pontuacao, IEnumerable<PerfilDeRisco> perfisDisponiveis);
    }

    public class RiskProfileService : IRiskProfileService
    {
        public PerfilDeRisco DeterminarPerfil(int pontuacao, IEnumerable<PerfilDeRisco> perfisDisponiveis)
        {
            var perfil = perfisDisponiveis.FirstOrDefault(p => p.PertenceAoIntervalo(pontuacao));
            if (perfil == null)
                throw new InvalidOperationException("Nenhum perfil corresponde à pontuação calculada.");
            return perfil;
        }
    }
}
