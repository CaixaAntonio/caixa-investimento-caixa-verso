using Microsoft.EntityFrameworkCore;
using Painel.investimento.Infra.Data;
using Painel.Investimento.Domain.Models;
using Painel.Investimento.Domain.Repository.Abstract;

namespace Painel.Investimento.Infra.Repositorie
{
    public class ClienteRepository : IClienteRepository
    {
        private readonly AppDbContext _context;

        public ClienteRepository(AppDbContext context)
        {
            _context = context;
        }


        public async Task AdicionarAsync(Cliente cliente)
        {
            await _context.Clientes.AddAsync(cliente);
            // Não chamamos SaveChanges aqui, pois o UnitOfWork é responsável pelo commit
        }

        public async Task<IEnumerable<Cliente>> ObterTodosAsync()
        {
            return await _context.Clientes
                                 .Include(c => c.Enderecos)
                                 .Include(c => c.Investimentos)                                 
                                 .ToListAsync();
        }

        public async Task<Cliente?> ObterPorIdAsync(int id)
        {
            return await _context.Clientes
                                 .Include(c => c.Enderecos)
                                 .Include(c => c.Investimentos)                                 
                                 .FirstOrDefaultAsync(c => c.Id == id);
        }

        public void Remover(Cliente cliente)
        {
            _context.Clientes.Remove(cliente);
            // Também não chamamos SaveChanges aqui, o UnitOfWork fará isso
        }

    }
}
