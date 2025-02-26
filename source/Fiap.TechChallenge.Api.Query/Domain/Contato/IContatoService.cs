using Fiap.TC03.Api.Consulta.Domain.Contato.Request;
using Fiap.TC03.Api.Consulta.Domain.Result;

namespace Fiap.TC03.Api.Consulta.Domain.Contato;

public interface IContatoService
{
    Task<ObterContatoPorIdResult> ObterContatoPorIdAsync(ObterContatoPorIdRequest request);
    Task<ObterContatosPorDddResult> ObterContatosPorDddAsync(ObterContatosPorDddRequest request);
}