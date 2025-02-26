using Fiap.TechChallenge.Api.Create.Domain.Contato.Request;
using Fiap.TechChallenge.Api.Create.Domain.Contato.Result;

namespace Fiap.TechChallenge.Api.Create.Domain.Contato;

public interface IContatoService
{
    Task<CriarContatoResult> CriarContatoAsync(CriarContatoRequest request);
}