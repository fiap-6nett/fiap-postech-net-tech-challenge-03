using Fiap.TechChallenge.Core.Contracts.Requests;
using Fiap.TechChallenge.Core.Contracts.Results;

namespace Fiap.TechChallenge.Core.Services
{
    public interface IContatoService
    {
        Task<CriarContatoResult> CriarContatoAsync(CriarContatoRequest request);
        Task<RemoverContatoResult> RemoverContatoAsync(RemoverContatoRequest request);
        Task<ObterContatoPorIdResult> ObterContatoPorIdAsync(ObterContatoPorIdRequest request);
        Task<ObterContatosPorDddResult> ObterContatosPorDddAsync(ObterContatosPorDddRequest request);
    }
}
