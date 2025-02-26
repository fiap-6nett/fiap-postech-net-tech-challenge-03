using FluentValidation;

namespace Fiap.TechChallenge.Api.Update.Domain.Contract;

public class AtualizarContatoCommandValidator : AbstractValidator<AtualizarContatoCommand>
{
    public AtualizarContatoCommandValidator()
    {
        // Validação para o Id (deve ser um GUID válido)
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("O ID do contato é obrigatório.")
            .Must(BeAValidGuid).WithMessage("O ID fornecido não é um GUID válido.");

        // Validação para o Nome (não pode ser vazio)
        RuleFor(x => x.Nome)
            .NotEmpty().WithMessage("O nome do contato é obrigatório.")
            .MinimumLength(2).WithMessage("O nome deve ter no mínimo 2 caracteres.")
            .MaximumLength(100).WithMessage("O nome pode ter no máximo 100 caracteres.");

        // Validação para o Telefone (não pode ser vazio)
        RuleFor(x => x.Telefone)
            .NotEmpty().WithMessage("O telefone do contato é obrigatório.")
            .Matches(@"^\d{8,15}$").WithMessage("O telefone deve ter entre 8 e 15 dígitos.");

        // Validação para o Email (deve ser um e-mail válido)
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("O e-mail do contato é obrigatório.")
            .EmailAddress().WithMessage("O e-mail fornecido não é válido.");

        // Validação para o DDD (deve estar entre 11 e 99)
        RuleFor(x => x.DDD)
            .NotEmpty().WithMessage("O DDD do contato é obrigatório.")
            .InclusiveBetween(11, 99).WithMessage("O DDD deve ser composto por dois dígitos entre 11 e 99.");
    }

    /// <summary>
    ///     Verifica se o Id fornecido é um GUID válido.
    /// </summary>
    /// <param name="id">O Id a ser validado.</param>
    /// <returns>True se for um GUID válido, False caso contrário.</returns>
    private bool BeAValidGuid(string id)
    {
        return Guid.TryParse(id, out _);
    }
}