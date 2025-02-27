namespace Fiap.TechChallenge.Core.Contracts.Requests
{
    public class RemoverContatoRequest
    {
        public RemoverContatoRequest(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }
    }
}
