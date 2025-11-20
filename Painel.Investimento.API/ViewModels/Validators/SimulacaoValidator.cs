using FluentValidation;
using Painel.Investimento.Domain.Models;


namespace Painel.investimento.API.ViewModels.Validators
{
    public class SimulacaoValidator : AbstractValidator<Simulacao>
    {
        public SimulacaoValidator()
        {
            RuleFor(x => x.ClienteId)
                .GreaterThan(0).WithMessage("ClienteId deve ser maior que zero.");

            RuleFor(x => x.NomeProduto)
                .NotEmpty().WithMessage("Nome do produto é obrigatório.")
                .MaximumLength(100).WithMessage("Nome do produto deve ter no máximo 100 caracteres.");

            RuleFor(x => x.ValorInicial)
                .GreaterThan(0).WithMessage("Valor inicial deve ser maior que zero.");

            RuleFor(x => x.ValorFinal)
                .GreaterThan(0).WithMessage("Valor final deve ser maior que zero.")
                .GreaterThanOrEqualTo(x => x.ValorInicial)
                .WithMessage("Valor final deve ser maior ou igual ao valor inicial.");

            RuleFor(x => x.PrazoMeses)
                .GreaterThan(0).WithMessage("Prazo em meses deve ser maior que zero.");

            RuleFor(x => x.DataSimulacao)
                .NotEmpty().WithMessage("Data da simulação é obrigatória.")
                .LessThanOrEqualTo(DateTime.UtcNow)
                .WithMessage("Data da simulação não pode ser futura.");

            RuleFor(x => x.RentabilidadeEfetiva)
                .GreaterThanOrEqualTo(-100).WithMessage("Rentabilidade não pode ser inferior a -100%.")
                .LessThanOrEqualTo(1000).WithMessage("Rentabilidade efetiva não pode ser superior a 1000%.");
        }
    }
}

