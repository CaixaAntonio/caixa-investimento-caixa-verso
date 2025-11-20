using Painel.Investimento.Domain.Dtos;
using Painel.Investimento.Domain.Models;
using Painel.Investimento.Domain.Repository.Abstract;

namespace Painel.Investimento.Aplication.useCaseSimulacoes
{
    public class SimularInvestimentoUseCase
    {
        private readonly IProdutoInvestimentoRepository _produtoRepo;
        private readonly ISimulacaoRepository _simulacaoRepo;
        private readonly IUnitOfWork _unitOfWork;

        public SimularInvestimentoUseCase(
            IProdutoInvestimentoRepository produtoRepo,
            ISimulacaoRepository simulacaoRepo,
            IUnitOfWork unitOfWork)
        {
            _produtoRepo = produtoRepo;
            _simulacaoRepo = simulacaoRepo;
            _unitOfWork = unitOfWork;
        }

        public async Task<SimulacaoInvestimentoResponse> ExecuteAsync(SimulacaoInvestimentoRequest request)
        {
            var produto = await _produtoRepo.GetByTipoAsync(request.NomeDoProduto);
            if (produto == null)
                throw new InvalidOperationException("Produto de investimento não encontrado.");

            produto.Validar();

            decimal valorFinal = 0;

            switch (produto.Tipo.ToLower())
            {
                case "poupança":
                    decimal taxaPoupanca = 0.005m;
                    valorFinal = request.Valor * (decimal)Math.Pow((double)(1 + taxaPoupanca), request.PrazoMeses);
                    break;

                case "cdb":
                case "lci":
                case "lca":
                    decimal taxaMensalCdb = produto.RentabilidadeAnual / 12 / 100;
                    valorFinal = request.Valor * (decimal)Math.Pow((double)(1 + taxaMensalCdb), request.PrazoMeses);
                    decimal ganhoCdb = valorFinal - request.Valor;
                    valorFinal -= ganhoCdb * 0.15m;
                    break;

                case "fundos renda fixa":
                    decimal taxaMensalRF = produto.RentabilidadeAnual / 12 / 100;
                    valorFinal = request.Valor * (decimal)Math.Pow((double)(1 + taxaMensalRF), request.PrazoMeses);
                    decimal ganhoRF = valorFinal - request.Valor;
                    valorFinal -= ganhoRF * 0.20m;
                    break;

                case "fundos multimercado":
                    decimal taxaMensalMM = produto.RentabilidadeAnual / 12 / 100;
                    valorFinal = request.Valor * (decimal)Math.Pow((double)(1 + taxaMensalMM), request.PrazoMeses);
                    decimal ganhoMM = valorFinal - request.Valor;
                    valorFinal -= ganhoMM * 0.20m;
                    break;

                case "fundos de ações":
                case "ações":
                case "home broker caixa":
                    decimal taxaMensalAcoes = produto.RentabilidadeAnual / 12 / 100;
                    valorFinal = request.Valor * (decimal)Math.Pow((double)(1 + taxaMensalAcoes), request.PrazoMeses);
                    decimal ganhoAcoes = valorFinal - request.Valor;
                    valorFinal -= ganhoAcoes * 0.15m;
                    break;

                default:
                    decimal taxaMensal = produto.RentabilidadeAnual / 12 / 100;
                    valorFinal = request.Valor * (decimal)Math.Pow((double)(1 + taxaMensal), request.PrazoMeses);
                    break;
            }

            var simulacao = new Simulacao(
                clienteId: request.ClienteId,
                nomeProduto: produto.Nome,
                valorInicial: request.Valor,
                prazoMeses: request.PrazoMeses,
                valorFinal: valorFinal
            );

            simulacao.Validar();

            await _simulacaoRepo.AddAsync(simulacao);
            await _unitOfWork.CommitAsync();


            // 6. Montar response
            var response = new SimulacaoInvestimentoResponse
            {
                ProdutoValidado = new ProdutoValidadoDto
                {
                    Id = produto.Id ?? 0,
                    Nome = produto.Nome,
                    Tipo = produto.Tipo,
                    Rentabilidade = produto.RentabilidadeAnual,
                    Risco = ConverterRisco(produto.Risco)
                },
                ResultadoSimulacao = new ResultadoSimulacaoDto
                {
                    ValorFinal = simulacao.ValorFinal,
                    RentabilidadeEfetiva = simulacao.RentabilidadeEfetiva,
                    PrazoMeses = simulacao.PrazoMeses
                },
                DataSimulacao = simulacao.DataSimulacao
            };

            return response;
        }
        public enum NivelRisco
        {
            Baixo = 10,
            Medio = 20,
            Alto = 30
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

        public async Task<RentabilidadeResponse> CalcularRentabilidadeAsync(int simulacaoId, decimal minimoPercentual)
        {
            var simulacao = await _simulacaoRepo.GetByIdAsync(simulacaoId);

            if (simulacao == null)
                throw new InvalidOperationException("Simulação não encontrada.");

            var percentual = simulacao.CalcularRentabilidadePercentual();
            var ehRentavel = simulacao.EhRentavel(minimoPercentual);

            return new RentabilidadeResponse
            {
                SimulacaoId = simulacao.Id,
                PercentualRentabilidade = percentual,
                EhRentavel = ehRentavel,
                ValorInicial = simulacao.ValorInicial,
                ValorFinal = simulacao.ValorFinal,
                RentabilidadeEfetiva = simulacao.RentabilidadeEfetiva,
                PrazoMeses = simulacao.PrazoMeses,
                DataSimulacao = simulacao.DataSimulacao
            };
        }

        /// <summary>
        /// Retorna todas as simulações realizadas
        /// </summary>
        public async Task<IEnumerable<SimulacaoResumoDto>> ListarTodasAsync()
        {
            var simulacoes = await _simulacaoRepo.GetAllAsync();

            return simulacoes.Select(s => new SimulacaoResumoDto
            {
                Id = s.Id,
                ClienteId = s.ClienteId,
                Produto = s.NomeProduto,
                ValorInvestido = s.ValorInicial,
                ValorFinal = s.ValorFinal,
                PrazoMeses = s.PrazoMeses,
                DataSimulacao = s.DataSimulacao
            });
        }


    }
}

