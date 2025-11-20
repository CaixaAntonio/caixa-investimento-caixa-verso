using Microsoft.EntityFrameworkCore;
using Painel.investimento.Infra.Data;
using Painel.Investimento.Domain.Models;
using Painel.Investimento.Domain.Repository.Abstract;

namespace Painel.Investimento.Infra.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly AppDbContext _context;

        public TransactionRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Transacao>> GetByClienteAsync(int clienteId)
        {
            return await _context.Transacoes
                .Where(t => t.ClienteId == clienteId)
                .ToListAsync();
        }

        public async Task<Transacao?> GetByIdAsync(int id)
        {
            return await _context.Transacoes.FindAsync(id);
        }

        public async Task AddAsync(Transacao transacao)
        {
            await _context.Transacoes.AddAsync(transacao);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Transacao transacao)
        {
            _context.Transacoes.Update(transacao);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var transacao = await _context.Transacoes.FindAsync(id);
            if (transacao != null)
            {
                _context.Transacoes.Remove(transacao);
                await _context.SaveChangesAsync();
            }
        }
    }
}
