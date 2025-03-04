using FluentValidation;

namespace Fiap.TechChallenge.Core.Contracts.Queries.QueryValidators
{
    /// <summary>
    ///     Validador para a requisição de obtenção de contatos por DDD.
    ///     Valida o valor do DDD, garantindo que seja composto por dois dígitos e que esteja dentro de um intervalo válido.
    /// </summary>
    public class ObterContatosPorDddQueryValidator : AbstractValidator<ObterContatosPorDddQueryRequest>
    {
        public ObterContatosPorDddQueryValidator()
        {
            RuleFor(x => x.Ddd)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("O DDD é obrigatório.")
                .InclusiveBetween(11, 99).WithMessage("O DDD deve ser composto por dois dígitos entre 11 e 99.");
        }
    }
}
