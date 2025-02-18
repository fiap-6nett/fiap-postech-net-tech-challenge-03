using Fiap.TechChallenge.Foundation.Core.Domain;

namespace Fiap.TC03.Infrastructure.Entities
{
    public sealed class ContatoEntity : EntityBase<Guid>
    {
        // Construtor padrão necessário para o Entity Framework Core
        public ContatoEntity()
        {
        }

        /// <summary>
        ///     Construtor que recebe todos os parâmetros da entidade.
        /// </summary>
        public ContatoEntity(Guid id, string nome, string telefone, string email, int ddd)
        {
            SetId(id);
            Nome = nome;
            Telefone = telefone;
            Email = email;
            DDD = ddd;
        }

        /// <summary>
        ///     Construtor sem o ID, útil para criar novas instâncias sem ID gerado previamente.
        /// </summary>
        public ContatoEntity(string nome, string telefone, string email, int ddd)
        {
            Nome = nome;
            Telefone = telefone;
            Email = email;
            DDD = ddd;
        }

        public string Nome { get; private set; }
        public string Telefone { get; private set; }
        public string Email { get; private set; }
        public int DDD { get; private set; }
    }
}
