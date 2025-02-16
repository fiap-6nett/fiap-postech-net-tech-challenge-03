using Fiap.TC03.Api.Exclusao.Domain.Contato.Request;
using Fiap.TC03.Api.Exclusao.Domain.Contato.Result;

namespace Fiap.TC03.Api.Exclusao.Domain.Contato;

public interface IContatoService
{
    Task<RemoverContatoResult> RemoverContatoAsync(RemoverContatoRequest request);

}