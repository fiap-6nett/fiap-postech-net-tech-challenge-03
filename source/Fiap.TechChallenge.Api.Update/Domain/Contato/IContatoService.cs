using Fiap.TechChallenge.Api.Update.Domain.Contato.Request;
using Fiap.TechChallenge.Api.Update.Domain.Contato.Result;

namespace Fiap.TechChallenge.Api.Update.Domain.Contato;

public interface IContatoService
{
    Task<AtualizarContatoResult> AtualizarContatoAsync(AtualizarContatoRequest request);

}