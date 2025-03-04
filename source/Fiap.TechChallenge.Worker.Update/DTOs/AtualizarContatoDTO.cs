namespace Fiap.TechChallenge.Worker.Update.DTOs
{
    public class AtualizarContatoDTO
    {
        public Guid Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Telefone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public int DDD { get; set; }
    }
}
