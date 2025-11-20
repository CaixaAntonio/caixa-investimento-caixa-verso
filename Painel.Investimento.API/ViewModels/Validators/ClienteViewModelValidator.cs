using FluentValidation;
using Painel.Investimento.Domain.Dtos;

namespace Painel.Investimento.API.ViewModels.Validators
{
    public class ClienteViewModelValidator : AbstractValidator<ClienteRequestDto>
    {
        public ClienteViewModelValidator()
        {
            // Nome e Sobrenome obrigatórios
            RuleFor(x => x.Nome)
                .NotEmpty().WithMessage("Nome é obrigatório.")
                .MaximumLength(100).WithMessage("Nome não pode ter mais que 100 caracteres.");

            RuleFor(x => x.Sobrenome)
                .NotEmpty().WithMessage("Sobrenome é obrigatório.")
                .MaximumLength(100).WithMessage("Sobrenome não pode ter mais que 100 caracteres.");

            // Email válido
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email é obrigatório.")
                .EmailAddress().WithMessage("Email inválido.");

            // Celular com 11 dígitos
            RuleFor(x => x.Celular)
                .NotEmpty().WithMessage("Celular é obrigatório.")
                .Matches(@"^\d{11}$").WithMessage("Celular deve ter 11 dígitos.");

            // CPF com 11 dígitos
            RuleFor(x => x.Cpf)
                .NotEmpty().WithMessage("CPF é obrigatório.")
                .Matches(@"^\d{11}$").WithMessage("CPF deve ter 11 dígitos.");

            // Data de nascimento não pode ser no futuro
            RuleFor(x => x.DataDeNascimento)
                .LessThan(DateTime.Today).WithMessage("Data de nascimento não pode ser no futuro.");

            // Perfil de risco obrigatório
            RuleFor(x => x.PerfilDeRiscoId)
                .NotEmpty().WithMessage("Perfil de risco é obrigatório.");
        }
    }
}
