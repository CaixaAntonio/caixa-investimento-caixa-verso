using Painel.Investimento.Domain.Models;

namespace Painel.Investimento.Domain.Repository.Abstract
{
    public interface IPerfilDeRiscoRepository
    {
        /// <summary>
        /// Retorna todos os perfis de risco disponíveis.
        /// </summary>
        Task<IEnumerable<PerfilDeRisco>> GetAllAsync();

        /// <summary>
        /// Retorna um perfil de risco pelo Id.
        /// </summary>
        Task<PerfilDeRisco?> GetByIdAsync(int id);

        /// <summary>
        /// Adiciona um novo perfil de risco.
        /// </summary>
        Task AddAsync(PerfilDeRisco perfil);

        /// <summary>
        /// Atualiza um perfil de risco existente.
        /// </summary>
        Task UpdateAsync(PerfilDeRisco perfil);

        /// <summary>
        /// Remove um perfil de risco.
        /// </summary>
        Task DeleteAsync(int id);
    }
}
