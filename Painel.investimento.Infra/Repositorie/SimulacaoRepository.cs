using Microsoft.EntityFrameworkCore;
using Painel.investimento.Infra.Data;
using Painel.Investimento.Domain.Dtos;
using Painel.Investimento.Domain.Models;
using Painel.Investimento.Domain.Repository.Abstract;

namespace Painel.Investimento.Infra.Repositories
{
    public class SimulacaoRepository : ISimulacaoRepository
    {
        private readonly AppDbContext _context;

        public SimulacaoRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Simulacao simulacao)
        {
            await _context.Simulacoes.AddAsync(simulacao);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<SimulacaoInvestimentoResponse>> GetByClienteIdAsync(int clienteId)
        {
            var simulacoes = await _context.Simulacoes
                                           .Where(s => s.ClienteId == clienteId)
                                           .ToListAsync();

            
            return simulacoes.Select(s => new SimulacaoInvestimentoResponse
            {
                ProdutoValidado = new ProdutoValidadoDto
                {
                    Id = 0, 
                    Nome = s.NomeProduto,
                    Tipo = "", 
                    Rentabilidade = 0,
                    Risco = ConverterRisco(0)
                },
                ResultadoSimulacao = new ResultadoSimulacaoDto
                {
                    ValorFinal = s.ValorFinal,
                    RentabilidadeEfetiva = s.RentabilidadeEfetiva,
                    PrazoMeses = s.PrazoMeses
                },
                DataSimulacao = s.DataSimulacao
            });
        }

        private string ConverterRisco(int risco)
        {
            return risco switch
            {
                10 => "Baixo",
                20 => "Médio",
                30 => "Alto",
                _ => "Desconhecido"
            };
        }

        public async Task<IEnumerable<SimulacaoPorDiaProdutoResponse>> GetSimulacoesAgrupadasAsync()
        {
            return await _context.Simulacoes
                .GroupBy(s => new { s.NomeProduto, Data = s.DataSimulacao.Date })
                .Select(g => new SimulacaoPorDiaProdutoResponse
                {
                    Produto = g.Key.NomeProduto,
                    Data = g.Key.Data,
                    QuantidadeSimulacoes = g.Count(),
                    MediaValorFinal = g.Average(s => s.ValorFinal)
                })
                .ToListAsync();
        }

        public async Task<Simulacao?> GetByIdAsync(int id)
        {
            return await _context.Simulacoes
                                 .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<IEnumerable<Simulacao>> GetAllAsync()
        {
            return await _context.Simulacoes.ToListAsync();
        }


    }
}
