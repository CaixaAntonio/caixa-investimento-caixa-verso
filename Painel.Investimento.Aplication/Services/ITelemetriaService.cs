using Painel.Investimento.Domain.Dtos.TelemetriaDto;
using System.Collections.Concurrent;

namespace Painel.Investimento.Application.Services
{
    public interface ITelemetriaService
    {
        void RegistrarChamada(string nomeServico, long tempoRespostaMs);
        TelemetriaResponse ObterRelatorio(DateTime inicio, DateTime fim);
    }



    public class TelemetriaService : ITelemetriaService
    {
        // 🔹 Cada serviço terá uma lista de registros (Data + TempoMs)
        private readonly ConcurrentDictionary<string, ConcurrentBag<(DateTime Data, long TempoMs)>> _metricas = new();

        public void RegistrarChamada(string nomeServico, long tempoRespostaMs)
        {
            var bag = _metricas.GetOrAdd(nomeServico, _ => new ConcurrentBag<(DateTime, long)>());
            bag.Add((DateTime.UtcNow, tempoRespostaMs));
        }

        public TelemetriaResponse ObterRelatorio(DateTime inicio, DateTime fim)
        {
            var servicos = _metricas.Select(m =>
            {
                // 🔹 Filtra apenas chamadas dentro do período
                var chamadasPeriodo = m.Value.Where(v => v.Data >= inicio && v.Data <= fim).ToList();

                return new TelemetriaServicoDto
                {
                    Nome = m.Key,
                    QuantidadeChamadas = chamadasPeriodo.Count,
                    MediaTempoRespostaMs = chamadasPeriodo.Any() ? chamadasPeriodo.Average(v => v.TempoMs) : 0
                };
            });

            return new TelemetriaResponse
            {
                Servicos = servicos,
                Periodo = new TelemetriaPeriodoDto
                {
                    Inicio = inicio,
                    Fim = fim
                }
            };
        }
    }

}
