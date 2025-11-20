using Microsoft.EntityFrameworkCore;
using Painel.investimento.Infra.Data;
using Painel.Investimento.Domain.Models;
using Painel.Investimento.Domain.Repository.Abstract;

namespace Painel.Investimento.Infra.Repositories
{
    public class PerfilDeRiscoRepository : IPerfilDeRiscoRepository
    {
        private readonly AppDbContext _context;

        public PerfilDeRiscoRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PerfilDeRisco>> GetAllAsync()
        {
            return await _context.PerfilDeRisco
                .Include(p => p.PerfilProdutos)
                .ToListAsync();
        }

        public async Task<PerfilDeRisco?> GetByIdAsync(int id)
        {
            return await _context.PerfilDeRisco
                .Include(p => p.PerfilProdutos)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task AddAsync(PerfilDeRisco perfil)
        {
            await _context.PerfilDeRisco.AddAsync(perfil);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(PerfilDeRisco perfil)
        {
            _context.PerfilDeRisco.Update(perfil);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var perfil = await _context.PerfilDeRisco.FindAsync(id);
            if (perfil != null)
            {
                _context.PerfilDeRisco.Remove(perfil);
                await _context.SaveChangesAsync();
            }
        }
    }
}
