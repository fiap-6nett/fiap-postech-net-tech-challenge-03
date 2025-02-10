using Fiap.TC03.Api.Cadastro.Domain.Contato.Request;
using Fiap.TC03.Api.Cadastro.Domain.Contato.Result;

namespace Fiap.TC03.Api.Cadastro.Domain.Contato;

public interface IContatoService
{
    Task<CriarContatoResult> CriarContatoAsync(CriarContatoRequest request);
}