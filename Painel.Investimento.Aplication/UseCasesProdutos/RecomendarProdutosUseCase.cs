using Painel.Investimento.Aplication.UseCaseInvestimentos;
using Painel.Investimento.Domain.Dtos;
using Painel.Investimento.Domain.Repository.Abstract;

namespace Painel.Investimento.Aplication.UseCasesProdutos
{
    public class RecomendarProdutosUseCase
    {
        private readonly CalcularPerfilDeRiscoUseCase _perfilUseCase;
        private readonly IProdutoInvestimentoRepository _produtoRepo;

        public RecomendarProdutosUseCase(
            CalcularPerfilDeRiscoUseCase perfilUseCase,
            IProdutoInvestimentoRepository produtoRepo)
        {
            _perfilUseCase = perfilUseCase;
            _produtoRepo = produtoRepo;
        }

        public async Task<IEnumerable<ProdutoRecomendadoDto>> ExecuteAsync(int clienteId)
        {
            // 1. Calcular perfil de risco do cliente
            var perfil = await _perfilUseCase.ExecuteAsync(clienteId);

            // 2. Buscar todos os produtos disponíveis
            var produtos = await _produtoRepo.GetAllAsync();

            // 3. Filtrar produtos de acordo com perfil
            var recomendados = produtos.Where(p =>
                perfil.Nome == "Conservador" && p.Risco == 10 ||
                perfil.Nome == "Moderado" && p.Risco == 20 ||
                perfil.Nome == "Agressivo" && p.Risco == 30// pode pegar todos
            );

            // 4. Mapear para DTO
            return recomendados.Select(p => new ProdutoRecomendadoDto
            {
                Id = (int)p.Id,
                Nome = p.Nome,
                Tipo = p.Tipo,
                Rentabilidade = p.RentabilidadeAnual,
                Risco = p.Risco switch
                {
                    10 => "Baixo",
                    20 => "Médio",
                    30 => "Alto",
                    _ => "Desconhecido"
                }
            });
        }
    }
}
