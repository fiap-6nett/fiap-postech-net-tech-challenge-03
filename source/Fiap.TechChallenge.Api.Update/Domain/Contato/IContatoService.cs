using Fiap.TC03.Api.Atualizacao.Domain.Contato.Request;
using Fiap.TC03.Api.Atualizacao.Domain.Contato.Result;

namespace Fiap.TC03.Api.Atualizacao.Domain.Contato;

public interface IContatoService
{
    Task<AtualizarContatoResult> AtualizarContatoAsync(AtualizarContatoRequest request);

}