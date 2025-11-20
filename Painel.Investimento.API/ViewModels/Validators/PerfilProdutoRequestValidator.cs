using FluentValidation;
using Painel.Investimento.Domain.Dtos;

namespace Painel.Investimento.API.ViewModels.Validators
{
    public class PerfilProdutoRequestValidator : AbstractValidator<PerfilProdutoRequestDto>
    {
        public PerfilProdutoRequestValidator()
        {
            RuleFor(p => p.PerfilDeRiscoId)
                .NotNull().WithMessage("O Perfil de Risco é obrigatório.")
                .GreaterThan(0).WithMessage("O Perfil de Risco deve ser maior que zero.");

            RuleFor(p => p.ProdutoInvestimentoId)
                .NotNull().WithMessage("O Produto de Investimento é obrigatório.")
                .GreaterThan(0).WithMessage("O Produto de Investimento deve ser maior que zero.");
        }
    }
}
