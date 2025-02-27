using Fiap.TechChallenge.Core.Contracts.Requests;
using Fiap.TechChallenge.Core.Contracts.Results;

namespace Fiap.TechChallenge.Core.Services
{
    public interface IContatoService
    {
        Task<CriarContatoResult> CriarContatoAsync(CriarContatoRequest request);
    }
}
