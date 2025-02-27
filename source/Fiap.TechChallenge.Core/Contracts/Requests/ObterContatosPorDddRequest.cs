namespace Fiap.TechChallenge.Core.Contracts.Requests
{
    public class ObterContatosPorDddRequest
    {
        public ObterContatosPorDddRequest(int ddd)
        {
            Ddd = ddd;
        }

        public int Ddd { get; set; }
    }
}
