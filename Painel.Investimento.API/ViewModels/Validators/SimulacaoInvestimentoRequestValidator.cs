using FluentValidation;
using Painel.Investimento.Domain.Dtos;

namespace Painel.Investimento.API.ViewModels.Validators
{
    public class SimulacaoInvestimentoRequestValidator : AbstractValidator<SimulacaoInvestimentoRequest>
    {
        public SimulacaoInvestimentoRequestValidator()
        {
            RuleFor(x => x.ClienteId)
                .GreaterThan(0).WithMessage("ClienteId deve ser informado e maior que zero.");

            RuleFor(x => x.Valor)
                .GreaterThan(0).WithMessage("O valor do investimento deve ser maior que zero.");

            RuleFor(x => x.PrazoMeses)
                .GreaterThan(0).WithMessage("O prazo em meses deve ser maior que zero.")
                .LessThanOrEqualTo(360).WithMessage("O prazo máximo permitido é de 360 meses.");

            RuleFor(x => x.NomeDoProduto)
                .NotEmpty().WithMessage("O campo NomeDoProduto é obrigatório.")
                .MaximumLength(100).WithMessage("NomeDoProduto não pode ultrapassar 100 caracteres.");
        }
    }
}
