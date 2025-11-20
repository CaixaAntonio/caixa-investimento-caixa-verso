
using FluentValidation;
using Painel.Investimento.Domain.Dtos;

namespace Painel.investimento.API.ViewModels.Validators
{   
    public class EnderecoRequestValidator : AbstractValidator<EnderecoRequest>
    {
        public EnderecoRequestValidator()
        {
            RuleFor(x => x.Logradouro)
                .NotEmpty().WithMessage("Logradouro é obrigatório.")
                .MaximumLength(150);

            RuleFor(x => x.Numero)
                .MaximumLength(10);

            RuleFor(x => x.Complemento)
                .MaximumLength(50);

            RuleFor(x => x.Bairro)
                .NotEmpty().WithMessage("Bairro é obrigatório.")
                .MaximumLength(100);

            RuleFor(x => x.Cidade)
                .NotEmpty().WithMessage("Cidade é obrigatória.")
                .MaximumLength(100);

            RuleFor(x => x.Estado)
                .NotEmpty().WithMessage("Estado é obrigatório.")
                .MaximumLength(50);

            RuleFor(x => x.Cep)
                .NotEmpty().WithMessage("CEP é obrigatório.")
                .Length(8).WithMessage("CEP deve ter 8 dígitos.")
                .Matches(@"^\d{8}$").WithMessage("CEP deve conter apenas números.");
        }
    }

}
