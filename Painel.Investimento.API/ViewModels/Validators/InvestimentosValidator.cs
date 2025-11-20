using FluentValidation;
using Painel.Investimento.Domain.Models;

namespace Painel.investimento.API.ViewModels.Validators
{
    public class InvestimentosValidator : AbstractValidator<Investimentos>
    {
        public InvestimentosValidator()
        {
            RuleFor(x => x.ClienteId)
                .NotNull().WithMessage("ClienteId é obrigatório.")
                .GreaterThan(0).WithMessage("ClienteId deve ser maior que zero.");

            RuleFor(x => x.ProdutoInvestimentoId)
                .GreaterThan(0).WithMessage("ProdutoInvestimentoId deve ser maior que zero.");

            RuleFor(x => x.ValorInvestido)
                .NotNull().WithMessage("ValorInvestido é obrigatório.")
                .GreaterThan(0).WithMessage("ValorInvestido deve ser maior que zero.");

            RuleFor(x => x.DataInvestimento)
                .NotNull().WithMessage("DataInvestimento é obrigatória.")
                .LessThanOrEqualTo(DateTime.Now).WithMessage("DataInvestimento não pode ser futura.");

            RuleFor(x => x.PrazoMeses)
                .GreaterThanOrEqualTo(0).When(x => x.PrazoMeses.HasValue)
                .WithMessage("PrazoMeses deve ser maior ou igual a zero.");

            RuleFor(x => x.ValorRetirado)
                .GreaterThanOrEqualTo(0).When(x => x.ValorRetirado.HasValue)
                .WithMessage("ValorRetirado deve ser maior ou igual a zero.");

            RuleFor(x => x)
                .Must(x => !x.ValorRetirado.HasValue || (x.ValorInvestido.HasValue && x.ValorRetirado <= x.ValorInvestido))
                .WithMessage("ValorRetirado não pode ser maior que ValorInvestido.");
        }
    }

}
