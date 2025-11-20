using Microsoft.EntityFrameworkCore;
using Painel.investimento.Infra.Data;
using Painel.Investimento.Domain.Models;
using Painel.Investimento.Domain.Repository.Abstract;

namespace Painel.Investimento.Infra.Repositories
{
    public class InvestimentosRepository : IInvestimentosRepository
    {
        private readonly AppDbContext _context;

        public InvestimentosRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Investimentos?> ObterPorIdAsync(int id)
        {
            return await _context.Investimentos
                                 .Include(i => i.Cliente)
                                 .Include(i => i.ProdutoInvestimento)
                                 .FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<IEnumerable<Investimentos>> ObterPorClienteAsync(int clienteId)
        {
            return await _context.Investimentos
                                 .Include(i => i.ProdutoInvestimento)
                                 .Where(i => i.ClienteId == clienteId)
                                 .ToListAsync();
        }

        public async Task<IEnumerable<Investimentos>> ObterInvestimentOldPorIdAsync(int clienteId, int produtoInvestID)
        {
            return await _context.Investimentos
                                 .Include(i => i.ProdutoInvestimento)
                                 .Where(i => i.ClienteId == clienteId && i.ProdutoInvestimentoId == produtoInvestID)
                                 .ToListAsync();
        }

        public async Task<IEnumerable<Investimentos>> ObterTodosAsync()
        {
            return await _context.Investimentos
                                 .Include(i => i.Cliente)
                                 .Include(i => i.ProdutoInvestimento)
                                 .ToListAsync();
        }

        public async Task AdicionarAsync(Investimentos investimento)
        {
            await _context.Investimentos.AddAsync(investimento);
            // SaveChanges será chamado pelo UnitOfWork
        }

        public async Task AtualizarAsync(Investimentos investimento)
        {
            _context.Investimentos.Update(investimento);
            await Task.CompletedTask; // SaveChanges será chamado pelo UnitOfWork
        }

        public void RemoverAsync(int id)
        {
            var investimento = _context.Investimentos.Find(id);
            if (investimento != null)
            {
                _context.Investimentos.Remove(investimento);
            }
            // SaveChanges será chamado pelo UnitOfWork
        }
    }
}
