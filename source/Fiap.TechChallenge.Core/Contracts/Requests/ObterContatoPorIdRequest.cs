namespace Fiap.TechChallenge.Core.Contracts.Requests
{
    public class ObterContatoPorIdRequest
    {
        public ObterContatoPorIdRequest(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }
    }
}
