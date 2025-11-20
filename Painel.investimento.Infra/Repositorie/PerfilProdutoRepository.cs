using Microsoft.EntityFrameworkCore;
using Painel.investimento.Infra.Data;
using Painel.Investimento.Domain.Models;
using Painel.Investimento.Domain.Repository.Abstract;

namespace Painel.Investimento.Infra.Repository
{
    public class PerfilProdutoRepository : IPerfilProdutoRepository
    {
        private readonly AppDbContext _context;
        private readonly DbSet<PerfilProduto> _dbSet;

        public PerfilProdutoRepository(AppDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<PerfilProduto>();
        }

        public async Task AddAsync(PerfilProduto perfilProduto)
        {
            await _dbSet.AddAsync(perfilProduto);
        }

        public async Task<PerfilProduto?> GetByIdsAsync(int perfilDeRiscoId, int produtoInvestimentoId)
        {
            return await _dbSet
                .Include(pp => pp.PerfilDeRisco)
                .Include(pp => pp.ProdutoInvestimento)
                .FirstOrDefaultAsync(pp =>
                    pp.PerfilDeRiscoId == perfilDeRiscoId &&
                    pp.ProdutoInvestimentoId == produtoInvestimentoId);
        }

        public async Task<IEnumerable<PerfilProduto>> GetAllAsync()
        {
            return await _dbSet
                .Include(pp => pp.PerfilDeRisco)
                .Include(pp => pp.ProdutoInvestimento)
                .ToListAsync();
        }

        public void Remove(PerfilProduto perfilProduto)
        {
            _dbSet.Remove(perfilProduto);
        }
    }
}
