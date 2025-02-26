using Fiap.TechChallenge.Api.Delete.Domain.Contato.Request;
using Fiap.TechChallenge.Api.Delete.Domain.Contato.Result;

namespace Fiap.TechChallenge.Api.Delete.Domain.Contato;

public interface IContatoService
{
    Task<RemoverContatoResult> RemoverContatoAsync(RemoverContatoRequest request);

}