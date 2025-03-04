using Fiap.TechChallenge.Core.Contracts.Requests;
using Fiap.TechChallenge.Core.Contracts.Results;

namespace Fiap.TechChallenge.Core.Services
{
    public interface IContatoService
    {
        #region Queue Methods

        Task<CriarContatoResult> CriarContatoQueueAsync(CriarContatoRequest request);
        Task<RemoverContatoResult> RemoverContatoQueueAsync(RemoverContatoRequest request);
        Task<ObterContatoPorIdResult> ObterContatoPorIdQueueAsync(ObterContatoPorIdRequest request);
        Task<ObterContatosPorDddResult> ObterContatosPorDddQueueAsync(ObterContatosPorDddRequest request);
        Task<AtualizarContatoResult> AtualizarContatoQueueAsync(AtualizarContatoRequest request);

        #endregion
    }
}
