using FluentValidation;

namespace Fiap.TC03.Api.Cadastro.Domain.Contract.CriarContato;

public class CriarContatoCommandValidator : AbstractValidator<CriarContatoCommand>
{
    public CriarContatoCommandValidator()
    {
        // Validação para o Nome (não pode ser vazio e deve ter entre 2 e 100 caracteres)
        RuleFor(x => x.Nome)
            .NotEmpty().WithMessage("O nome do contato é obrigatório.")
            .MinimumLength(2).WithMessage("O nome deve ter no mínimo 2 caracteres.")
            .MaximumLength(100).WithMessage("O nome pode ter no máximo 100 caracteres.");

        // Validação para o Telefone (não pode ser vazio e deve ter entre 8 e 15 dígitos)
        RuleFor(x => x.Telefone)
            .NotEmpty().WithMessage("O telefone do contato é obrigatório.")
            .Matches(@"^\d{8,15}$").WithMessage("O telefone deve ter entre 8 e 15 dígitos.");

        // Validação para o Email (não pode ser vazio e deve ser um e-mail válido)
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("O e-mail do contato é obrigatório.")
            .EmailAddress().WithMessage("O e-mail fornecido não é válido.");

        // Validação para o DDD (deve ser um número entre 11 e 99)
        RuleFor(x => x.DDD)
            .InclusiveBetween(11, 99).WithMessage("O DDD deve ser composto por dois dígitos entre 11 e 99.");
    }
}