using FluentValidation;
using Painel.Investimento.Domain.Models;


namespace Painel.investimento.API.ViewModels.Validators
{
    public class PerfilDeRiscoValidator : AbstractValidator<PerfilDeRisco>
    {
        public PerfilDeRiscoValidator()
        {
            RuleFor(x => x.Nome)
                .NotEmpty().WithMessage("Nome do perfil é obrigatório.")
                .MaximumLength(100).WithMessage("Nome do perfil deve ter no máximo 100 caracteres.");

            RuleFor(x => x.PontuacaoMinima)
                .GreaterThanOrEqualTo(0).WithMessage("Pontuação mínima não pode ser negativa.");

            RuleFor(x => x.PontuacaoMaxima)
                .GreaterThan(x => x.PontuacaoMinima)
                .WithMessage("Pontuação máxima deve ser maior que a mínima.");

            RuleFor(x => x.Descricao)
                .NotEmpty().WithMessage("Descrição é obrigatória.")
                .MaximumLength(250).WithMessage("Descrição deve ter no máximo 250 caracteres.");

            RuleForEach(x => x.PerfilProdutos)
                .NotNull().WithMessage("PerfilProduto não pode ser nulo.");
        }
    }

}
