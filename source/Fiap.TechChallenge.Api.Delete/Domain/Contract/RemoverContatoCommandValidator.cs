﻿using FluentValidation;

namespace Fiap.TechChallenge.Api.Delete.Domain.Contract;

public class RemoverContatoCommandValidator : AbstractValidator<RemoverContatoCommand>
{
    public RemoverContatoCommandValidator()
    {
        RuleFor(x => x.Id)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("O ID do contato é obrigatório.")
            .Must(BeAValidGuid).WithMessage("O ID fornecido não é um GUID válido.");
    }

    /// <summary>
    ///     Verifica se a string fornecida pode ser convertida em um GUID válido.
    /// </summary>
    /// <param name="id">O ID a ser validado.</param>
    /// <returns>True se o ID for um GUID válido, caso contrário, False.</returns>
    private bool BeAValidGuid(string id)
    {
        return Guid.TryParse(id, out _);
    }
}