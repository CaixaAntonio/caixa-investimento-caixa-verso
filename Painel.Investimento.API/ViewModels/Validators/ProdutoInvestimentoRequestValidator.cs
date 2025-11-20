using FluentValidation;
using Painel.Investimento.Domain.Dtos;

namespace Painel.Investimento.API.ViewModels.Validators
{
    public class ProdutoInvestimentoRequestValidator : AbstractValidator<ProdutoInvestimentoRequestDto>
    {
        public ProdutoInvestimentoRequestValidator()
        {
            RuleFor(p => p.Nome)
                .NotEmpty().WithMessage("O nome do produto é obrigatório.")
                .MaximumLength(100).WithMessage("O nome não pode ultrapassar 100 caracteres.");

            RuleFor(p => p.Tipo)
                .NotEmpty().WithMessage("O tipo de perfil do produto é obrigatório.")
                .MaximumLength(50).WithMessage("O tipo de perfil não pode ultrapassar 50 caracteres.");

            RuleFor(p => p.RentabilidadeAnual)
                .NotNull().WithMessage("A rentabilidade anual é obrigatória.")
                .GreaterThan(0).WithMessage("A rentabilidade deve ser maior que zero.")
                .LessThanOrEqualTo(100).WithMessage("A rentabilidade não pode ser maior que 100%.");

            RuleFor(p => p.Risco)
                .NotNull().WithMessage("O risco é obrigatório.")
                .InclusiveBetween(1, 100).WithMessage("O risco deve estar entre 1 e 100.");

            RuleFor(p => p.Liquidez)
                .NotEmpty().WithMessage("A liquidez é obrigatória.")
                .MaximumLength(50).WithMessage("A liquidez não pode ultrapassar 50 caracteres.");

            RuleFor(p => p.Tributacao)
                .NotEmpty().WithMessage("A tributação é obrigatória.")
                .MaximumLength(100).WithMessage("A tributação não pode ultrapassar 100 caracteres.");

            RuleFor(p => p.Garantia)
                .NotEmpty().WithMessage("A garantia é obrigatória.")
                .MaximumLength(100).WithMessage("A garantia não pode ultrapassar 100 caracteres.");

            RuleFor(p => p.Descricao)
                .NotEmpty().WithMessage("A descrição é obrigatória.")
                .MaximumLength(500).WithMessage("A descrição não pode ultrapassar 500 caracteres.");
        }
    }
}
