using Fiap.TechChallenge.Core.Entities;

namespace Fiap.TechChallenge.Core.Contracts.Results
{
    public class ObterContatosPorDddResult
    {
        public ObterContatosPorDddResult(List<ContatoEntity> contatos)
        {
            Contatos = contatos;
        }

        public List<ContatoEntity> Contatos { get; set; }
    }
}
