using Fiap.TechChallenge.Api.Query.Domain.Contato.Request;
using Fiap.TechChallenge.Api.Query.Domain.Result;

namespace Fiap.TechChallenge.Api.Query.Domain.Contato;

public interface IContatoService
{
    Task<ObterContatoPorIdResult> ObterContatoPorIdAsync(ObterContatoPorIdRequest request);
    Task<ObterContatosPorDddResult> ObterContatosPorDddAsync(ObterContatosPorDddRequest request);
}