namespace Fiap.TechChallenge.Worker.Create.DTOs
{
    public class CriarContatoDTO
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public string Telefone { get; set; }
        public string Email { get; set; }
        public int DDD { get; set; }
    }
}
