using Microsoft.EntityFrameworkCore;
using Painel.investimento.Infra.Data;
using Painel.Investimento.Domain.Models;
using Painel.Investimento.Domain.Repository.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Painel.investimento.Infra.Repositorie
{
    public class ProdutoInvestimentoRepository : IProdutoInvestimentoRepository
    {
        private readonly AppDbContext _context;

        public ProdutoInvestimentoRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(ProdutoInvestimento produto)
        {
            await _context.ProdutosInvestimento.AddAsync(produto);
            // Não chamamos SaveChanges aqui, pois o UnitOfWork é responsável pelo commit
        }

        public async Task<IEnumerable<ProdutoInvestimento>> GetAllAsync()
        {
            return await _context.ProdutosInvestimento.ToListAsync();
        }

        public async Task<ProdutoInvestimento?> GetByIdAsync(int id)
        {
            return await _context.ProdutosInvestimento
                                 .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<ProdutoInvestimento?> ObterPorIdAsync(int id)
        {
            return await _context.ProdutosInvestimento
                                .FirstOrDefaultAsync(p => p.Id == id);
        }

       
        public async Task<ProdutoInvestimento?> GetByTipoAsync(string nomeProduto)
        {
            return await _context.ProdutosInvestimento
                                 .FirstOrDefaultAsync(p => p.Nome == nomeProduto);
        }

        public void Remove(ProdutoInvestimento produto)
        {
            _context.ProdutosInvestimento.Remove(produto);
            // Também não chamamos SaveChanges aqui, o UnitOfWork fará isso
        }
    }
}
