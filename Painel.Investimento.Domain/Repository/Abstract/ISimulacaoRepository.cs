using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Painel.Investimento.Domain.Dtos;
using Painel.Investimento.Domain.Models;

namespace Painel.Investimento.Domain.Repository.Abstract
{
    public interface ISimulacaoRepository
    {
        Task AddAsync(Simulacao simulacao);
        Task<IEnumerable<SimulacaoInvestimentoResponse>> GetByClienteIdAsync(int clienteId);
        Task<Simulacao?> GetByIdAsync(int id);
        Task<IEnumerable<Simulacao?>> GetAllAsync();
        Task<IEnumerable<SimulacaoPorDiaProdutoResponse>> GetSimulacoesAgrupadasAsync();
    }

}
